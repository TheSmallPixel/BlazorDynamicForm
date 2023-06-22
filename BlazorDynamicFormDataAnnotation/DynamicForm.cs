using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Dynamic;
using System.Linq.Expressions;
using BlazorDynamicFormGenerator;
using System.Reflection;

namespace BlazorDynamicFormDataAnnotation
{
    public class DynamicForm : ComponentBase
    {
        [Parameter]
        public ExpandoObject DataObject { get; set; }

        [Parameter]
        public ModuleNodeDefinition DataDefinition { get; set; }

        [Parameter]
        public EventCallback<string> OnValidSubmit { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public RenderFragment<string>? ValidationMessageTemplate { get; set; }
        [Parameter]
        public RenderFragment<FieldTemplateContext> FieldTemplate { get; set; }
        [Parameter]
        public RenderFragment SubmitTemplate { get; set; }

        [Inject]
        private DynamicFormConfiguration Configuration { get; set; }

        private readonly Dictionary<string, Action<object>> _updateActions = new();
        private Dictionary<string, List<string>> _validationMessages = new();

        public class FieldTemplateContext
        {
            public FieldTemplateContext(RenderFragment dynamicComponent, RenderFragment validationComponent, bool isValid, ModuleNodePropertyDefinition propertyInfo)
            {
                DynamicComponent = dynamicComponent;
                ValidationComponent = validationComponent;
                IsValid = isValid;
                PropertyInfo = propertyInfo;
            }

            public RenderFragment DynamicComponent { get; private set; }
            public RenderFragment ValidationComponent { get; private set; }
            public bool IsValid { get; private set; }
            public ModuleNodePropertyDefinition PropertyInfo { get; private set; }
        }

        private RenderFragment GetBuildContent() => PopulateFormContent;

        private RenderFragment AddComponent(ModuleNodePropertyDefinition property, IDictionary<string, object> dataDictionary) => builder =>
        {
            OpenComponent(builder, property, dataDictionary);
        };
        private RenderFragment AddValidation(ModuleNodePropertyDefinition property) => builder =>
        {
            DisplayValidationMessages(builder, property);
        };

        protected override void OnInitialized()
        {
            DataObject ??= new ExpandoObject();
            var dataDictionary = DataObject as IDictionary<string, object>;
            foreach (var property in DataDefinition.PropertyDefinitions)
            {
                if (dataDictionary.ContainsKey(property.Name)) continue;
                if (!Configuration.FormComponents.TryGetValue(((property.CustomDataType ?? property.DataType.ToString()) ?? "Default"), out var value)) continue;
                dataDictionary.TryAdd(property.Name,
                    property.DefaultValue != null ? Convert.ChangeType(property.DefaultValue, Nullable.GetUnderlyingType(value.DataType) ?? value.DataType) : null);
            }

            base.OnInitialized();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenRegion(GetHashCode());
            builder.OpenElement(0, "form");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "onsubmit", HandleSubmitAsync);
            builder.AddAttribute(3, "novalidate");
            GetBuildContent()(builder);
            builder.AddContent(4, SubmitTemplate);
            builder.CloseElement();
            builder.CloseRegion();
        }

        private void PopulateFormContent(RenderTreeBuilder builder)
        {
            var dataDictionary = DataObject as IDictionary<string, object>;
            builder.OpenRegion(0);
            foreach (var property in DataDefinition.PropertyDefinitions)
            {
                var isValid = _validationMessages.All(x => x.Key != property.Name);
                var template = new FieldTemplateContext(AddComponent(property, dataDictionary), AddValidation(property), isValid, property);

                builder.AddContent(1, FieldTemplate(template));
                //OpenComponent(builder, property, dataDictionary);
            }
            builder.CloseRegion();
        }

        private void OpenComponent(RenderTreeBuilder builder, ModuleNodePropertyDefinition property, IDictionary<string, object> dataDictionary)
        {
            var sequence = new Sequence();
            _updateActions[property.Name] = newValue =>
            {
                dataDictionary[property.Name] = newValue;
                _validationMessages.Clear();
                ValidateData();
                StateHasChanged();
            };

            if (!Configuration.FormComponents.TryGetValue(((property.CustomDataType ?? property.DataType.ToString()) ?? "Default"), out var value)) return;
            builder.OpenComponent(sequence++, value.ComponentType);
            value.DynamicFragment?.Invoke(builder, sequence, property);

            builder.AddAttribute(sequence++, "ValueChanged", CreateEventCallback(property, value));

            builder.AddAttribute(sequence++, "Value", dataDictionary[property.Name]);
            builder.AddAttribute(sequence++, "Placeholder", property.DisplayName);

            builder.CloseComponent();
        }

        private void DisplayValidationMessages(RenderTreeBuilder builder, ModuleNodePropertyDefinition property)
        {
            if (!_validationMessages.TryGetValue(property.Name, out var messages)) return;
            if (ValidationMessageTemplate == null) return;
            builder.OpenRegion(0);
            foreach (var message in messages)
            {
                builder.AddContent(1, ValidationMessageTemplate(message));
            }
            builder.CloseRegion();
        }

        private async Task HandleSubmitAsync()
        {
            _validationMessages.Clear();
            ValidateData();
            if (IsDataValid())
            {
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(DataObject);
                await OnValidSubmit.InvokeAsync(data);
            }
            StateHasChanged();
        }

        private bool IsDataValid()
        {
            return _validationMessages.Count == 0;
        }

        private void ValidateData()
        {
            var validationRules = DataDefinition.PropertyDefinitions.ToDictionary(x => x.Name, x => x.ValidationRules);
            _validationMessages = ValidateDataAgainstRules(DataObject, validationRules);
        }

        private Dictionary<string, List<string>> ValidateDataAgainstRules(ExpandoObject data, Dictionary<string, List<ValidationAttribute>> validationRules)
        {
            var validationResults = new Dictionary<string, List<string>>();
            var context = new ValidationContext(data);
            foreach (var pair in data)
            {
                if (!validationRules.TryGetValue(pair.Key, out var attributes)) continue;
                var validationResult = ValidateProperty(attributes, pair);
                if (validationResult.Count > 0)
                {
                    validationResults.Add(pair.Key, validationResult);
                }
            }
            return validationResults;
        }

        private List<string> ValidateProperty(List<ValidationAttribute> attributes, KeyValuePair<string, object> property)
        {
            List<string> validationResult = new();
            foreach (var attribute in attributes)
            {
                if (attribute.IsValid(property.Value)) continue;
                validationResult.Add(new ValidationResult(attribute.FormatErrorMessage(property.Key), new[] { property.Key }).ErrorMessage ?? string.Empty);
            }
            return validationResult ?? new List<string>();
        }

        private object? CreateEventCallback(ModuleNodePropertyDefinition property, DynamicFormElement element)
        {
            Type dynamicType = element.DataType;
            var actionType = typeof(Action<>).MakeGenericType(dynamicType);
            var paramExpr = Expression.Parameter(dynamicType);
            var exp = Expression.Invoke(Expression.Constant(_updateActions[property.Name]), Expression.Convert(paramExpr, typeof(object)));
            var lambda = Expression.Lambda(actionType, exp, paramExpr);
            var createMethod = EventCallbackFactoryCreateMethod.MakeGenericMethod(dynamicType);
            return createMethod.Invoke(EventCallback.Factory, new object[] { this, lambda.Compile() });
        }

        private static readonly MethodInfo EventCallbackFactoryCreateMethod = GetEventCallbackFactoryCreateMethod();

        private static MethodInfo GetEventCallbackFactoryCreateMethod()
        {
            return typeof(EventCallbackFactory).GetMethods()
                .Single(m => m.Name == "Create" && m.IsPublic && !m.IsStatic && m.IsGenericMethod
                             && m.GetGenericArguments().Length == 1
                             && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == typeof(object)
                             && m.GetParameters()[1].ParameterType.IsGenericType && m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>));
        }
    }
}

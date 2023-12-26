using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using BlazorDynamicForm.Validation;
using DataTypeAttribute = BlazorDynamicForm.Validation.DataTypeAttribute;
using Newtonsoft.Json.Linq;
using static BlazorDynamicForm.Validation.DataTypeAttribute;

namespace BlazorDynamicForm
{
    public class DynamicForm : ComponentBase
    {
        [Parameter]
        public ExpandoObject? DataObject { get; set; }

        [Parameter]
        public FormMap? FormDefinition { get; set; }

        [Parameter]
        public EventCallback<ExpandoObject?> OnValidSubmit { get; set; }

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

        private Dictionary<string, List<string>> _validationMessages = new();

        public class FieldTemplateContext
        {
            public FieldTemplateContext(RenderFragment dynamicComponent, RenderFragment validationComponent, bool isValid, FormProperty propertyInfo)
            {
                DynamicComponent = dynamicComponent;
                ValidationComponent = validationComponent;
                IsValid = isValid;
                PropertyInfo = propertyInfo;
            }

            public RenderFragment DynamicComponent { get; private set; }
            public RenderFragment ValidationComponent { get; private set; }
            public bool IsValid { get; private set; }
            public FormProperty PropertyInfo { get; private set; }
        }

        private RenderFragment GetBuildContent() => PopulateFormContent;

        private RenderFragment AddComponent(FormProperty property, IDictionary<string, object> dataDictionary, string key, string currentKey) => builder =>
        {
            OpenComponent(builder, property, dataDictionary, key, currentKey);
        };
        private RenderFragment AddValidation(FormProperty property) => builder =>
        {
            DisplayValidationMessages(builder, property);
        };

        protected override void OnInitialized()
        {
            DataObject ??= new ExpandoObject();
            var dataDictionary = DataObject as IDictionary<string, object?>;
            if (FormDefinition != null)
            {
                FirstLayerDataSetup(FormDefinition.EntryType, dataDictionary);
            }

            base.OnInitialized();
        }

        private void FirstLayerDataSetup(string key, IDictionary<string, object?> dataDictionary)
        {
            if (FormDefinition != null && !FormDefinition.Properties.ContainsKey(key))
            {
                //FISHY  return error in case
                return;
            }

            foreach (var property in FormDefinition.Properties[key].Structure)
            {
                if (property.Value == "Int32" || property.Value == "String")
                {
                    dataDictionary.TryAdd(property.Key, property.Value == "String" ? string.Empty : 0);
                }
                else
                {
                    var prop = FormDefinition.Properties[property.Value];

                    switch (prop.PropertyType)
                    {
                        case FormPropertyType.Primitive:
                            dataDictionary.TryAdd(property.Key, prop.TypeName == "String" ? string.Empty : 0);
                            break;
                        case FormPropertyType.Object:
                            var value = new ExpandoObject();
                            dataDictionary.TryAdd(property.Key, value);
                            //DynamicSetup(property.Value, value);   no this is wrong can create infinite loops

                            break;
                        case FormPropertyType.Collection when prop.TypeName == "List":
                            dataDictionary.TryAdd(property.Key, new List<ExpandoObject>());
                            break;
                        case FormPropertyType.Collection when prop.TypeName == "Dictionary":
                            dataDictionary.TryAdd(property.Key, new List<ExpandoObject>());
                            break;
                        case FormPropertyType.Collection:
                            //TODO not supported
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
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
            if (FormDefinition == null || dataDictionary == null) return;
            builder.OpenRegion(0);

            foreach (var frag in CreateComponents(builder, FormDefinition.EntryType, dataDictionary))
            {
                builder.AddContent(1, frag);
            }
            builder.CloseRegion();
        }

        private List<RenderFragment> CreateComponents(RenderTreeBuilder builder, string key, IDictionary<string, object> dataDictionary)
        {
            List<RenderFragment> returFrags = new();
            if (FormDefinition.Properties.ContainsKey(key))
            {
                var currentProp = FormDefinition.Properties[key];
                foreach (var child in currentProp.Structure)
                {
                    if (FormDefinition.Properties.ContainsKey(child.Value))
                    {
                        //this is a nested component mmhh
                        var innerKey = FormDefinition.Properties[child.Value];
                        var isValid = _validationMessages.All(x => x.Key != child.Key);
                        var template = new FieldTemplateContext(AddComponent(innerKey, dataDictionary, child.Key, child.Value), AddValidation(innerKey), isValid, innerKey);

                        returFrags.Add(FieldTemplate(template));
                    }
                    else
                    {
                        //this is a simpleone
                        //TODO add primitive                            
                        //var isValid = _validationMessages.All(x => x.Key != property.Key);
                        var template = new FieldTemplateContext(AddComponent(currentProp, dataDictionary, child.Key, child.Value), AddValidation(currentProp), true, currentProp);

                        returFrags.Add(FieldTemplate(template));
                    }
                }
            }
            else
            {
                //This is an error only containers can call this
                throw new InvalidOperationException("You can only create components from Properties!");
            }


            return returFrags;
        }

        public void HandleChange(object data)
        {
            
        }

        private void OpenComponent(RenderTreeBuilder builder, FormProperty property, IDictionary<string, object> dataDictionary, string key, string currentKey)
        {
            if (dataDictionary == null) return;
            var sequence = new Sequence();
           
            var displayType = property.DisplayRules.OfType<DataTypeAttribute>().FirstOrDefault();
            var defaultValue = property.DisplayRules.OfType<DefaultValueAttribute>().FirstOrDefault();
            var displayName = property.DisplayRules.OfType<DisplayNameForm>().FirstOrDefault()?.Name ?? property.TypeName;
            FormDatatype type = FormDatatype.String;
            if (property.PropertyType == FormPropertyType.Primitive)
            {
                if (property.TypeName == "String")
                    type = FormDatatype.String;
                if (property.TypeName == "Int32")
                    type = FormDatatype.Integer;
            }
            else if (property.PropertyType == FormPropertyType.Object)
            {
                type = FormDatatype.Object;
            }
            else if (property.PropertyType == FormPropertyType.Collection)
            {
                type = FormDatatype.List;
            }
            if (displayType != null)
            {
                type = displayType.Type;
            }

            Action<object> dynamicEventCallback = newValue => {
                dataDictionary[key] = newValue;
                _validationMessages.Clear();
                ValidateData();
                StateHasChanged();
            };
         

            if (Configuration.FormComponents.TryGetValue(type.ToString(), out var value))
            {
                builder.OpenComponent(sequence++, value.ComponentType);
                if (property.PropertyType == FormPropertyType.Collection || property.PropertyType == FormPropertyType.Object)
                {
                    Func<string, object, List<RenderFragment>> opeFunc = (childKey, dataChild) =>
                    {
                        if (dataChild == null)
                        {
                            dataChild = new ExpandoObject();
                        }
                        if (dataChild is IDictionary<string, object?> dataDick)
                        {
                            FirstLayerDataSetup(childKey, dataChild as IDictionary<string, object?>);
                        }
                        return CreateComponents(builder, childKey, dataChild as IDictionary<string, object?>);
                    };                                                                                                                                 
                    value.DynamicContentFragment?.Invoke(builder, FormDefinition, currentKey, sequence, property, opeFunc, dataDictionary[key], CreateEventCallback(dynamicEventCallback, property, value));
                }
                else
                {
                    value.DynamicFragment?.Invoke(builder, sequence, property, dataDictionary[key], CreateEventCallback(dynamicEventCallback, property, value));
                }
                builder.CloseComponent();
            }
        }

        private void DisplayValidationMessages(RenderTreeBuilder builder, FormProperty property)
        {
            if (!_validationMessages.TryGetValue(property.TypeName, out var messages)) return;
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
                
                await OnValidSubmit.InvokeAsync(DataObject);
            }
            StateHasChanged();
        }

        private bool IsDataValid()
        {
            return _validationMessages.Count == 0;
        }

        private void ValidateData()
        {
            //TODO
            var validationRules = FormDefinition.Properties.ToDictionary(x => x.Key, x => x.Value.ValidationRules);
            _validationMessages = ValidateDataAgainstRules(DataObject, validationRules);
        }

        private Dictionary<string, List<string>> ValidateDataAgainstRules(ExpandoObject data, Dictionary<string, List<ValidationRule>> validationRules)
        {
            var validationResults = new Dictionary<string, List<string>>();
            //var context = new ValidationContext(data);
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

        private List<string> ValidateProperty(List<ValidationRule> attributes, KeyValuePair<string, object> property)
        {
            List<string> validationResult = new();
            foreach (var attribute in attributes)
            {
                if (attribute.IsValid(FormDefinition, property.Value)) continue;
                //TODO validationResult.Add(new ValidationResult(attribute.FormatErrorMessage(property.Key), new[] { property.Key }).ErrorMessage ?? string.Empty);
            }
            return validationResult ?? new List<string>();
        }

        private object? CreateEventCallback(Action<object> action, FormProperty property, DynamicFormElement element)
        {
            Type dynamicType = element.ComponentValueType;
            var actionType = typeof(Action<>).MakeGenericType(dynamicType);
            var paramExpr = Expression.Parameter(dynamicType);
            var exp = Expression.Invoke(Expression.Constant(action), Expression.Convert(paramExpr, typeof(object)));
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

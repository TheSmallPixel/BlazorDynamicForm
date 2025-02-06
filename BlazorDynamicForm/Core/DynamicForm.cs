//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Rendering;
//using System.Dynamic;
//using BlazorDynamicForm.Utility;
//using TypeAnnotationParser;

//namespace BlazorDynamicForm.Core
//{
//    public class DynamicForm : ComponentBase
//    {
//        [Parameter]
//        public ExpandoObject DataObject { get; set; }

//        [Parameter]
//        public SchemeModel FormDefinition { get; set; }

//        [Parameter]
//        public EventCallback<ExpandoObject?> OnValidSubmit { get; set; }

//        [Parameter(CaptureUnmatchedValues = true)]
//        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

//        [Parameter]
//        public RenderFragment<FieldTemplateContext> FieldTemplate { get; set; }

//        [Parameter]
//        public RenderFragment SubmitTemplate { get; set; }

//        [Inject]
//        private DynamicFormConfiguration Configuration { get; set; }

//        protected override void BuildRenderTree(RenderTreeBuilder builder)
//        {
//            builder.OpenRegion(GetHashCode());
//            builder.OpenElement(0, "form");
//            builder.AddMultipleAttributes(1, AdditionalAttributes);
//            builder.AddAttribute(2, "onsubmit", HandleSubmitAsync);
//            builder.AddAttribute(3, "novalidate");
//            PopulateFormContent(builder);
//            builder.AddContent(4, SubmitTemplate);
//            builder.CloseElement();
//            builder.CloseRegion();
//        }

//        private void PopulateFormContent(RenderTreeBuilder builder)
//        {
//            DataObject ??= new ExpandoObject();
//            var dataDictionary = DataObject as IDictionary<string, object>;
//            builder.OpenRegion(0);
//            builder.AddContent(1, CreateComponents(FormDefinition, FormDefinition, dataDictionary, true));
//            builder.CloseRegion();
//        }

//        public object? CreateObject(string key)
//        {
//            Console.WriteLine($"Creating {key}..");
//            return FormDefinition.CreateObject(key, (option) =>
//            {
//                option.InitStringsEmpty = true;
//                option.MaxRecursiveDepth = 10;
//                option.CreateCollectionElement = false;
//                option.CreateDictionaryElement = false;
//                option.CreateObjectElement = false;
//            });
//        }

//        public RenderFragment CreateComponents(SchemeModel schemeModel, SchemeProperty schemeProperty, object container, bool isFirst = false)
//        {
//	        if (schemeProperty.Properties is null)
//		        return null;

//            object? value = null;
//            if (container is IDictionary<string, object> dict)
//            {
//                if (!dict.TryGetValue(propertyName, out value))
//                {
//                    value = CreateObject(key);
//                    dict[propertyName] = value;
//                }
//            }
//            else
//            {
//                throw new ArgumentException("Container type or property name type is not supported or does not match.");
//            }

//            var sequence = new Sequence();
//            Action<object> updateContent = (updateData) =>
//            {
//                if (container is IDictionary<string, object> dict)
//                {
//                    dict[propertyName] = updateData;
//                }
//            };

//            void Component(RenderTreeBuilder builder)
//            {
//                var component = Configuration.GetElement(property);
//                if (component == null)
//                    return;
//                builder.OpenComponent(sequence++, component);
//                builder.AddAttribute(sequence++, "Form", this);
//                builder.AddAttribute(sequence++, "PropertyName", propertyName);
//                builder.AddAttribute(sequence++, "PropertyType", key);
//                builder.AddAttribute(sequence++, "IsFirst", isFirst);
//                builder.AddAttribute(sequence++, "ValueChanged", updateContent);
//                builder.AddAttribute(sequence++, "Value", value);
//                builder.CloseComponent();
//            }

//            return FieldTemplate(new FieldTemplateContext(property, Component));
//        }

//        public RenderFragment CreateComponents(string key, int index, object container, bool isFirst = false)
//        {
//            if (!FormDefinition.Properties.TryGetValue(key, out var property))
//                throw new InvalidOperationException($"The {key} is not in the Form Definition");
//            object? value = null;
//            if (container is IList<object> list)
//            {
//                if (index >= 0 && index < list.Count)
//                {
//                    value = list[index];
//                }
//                else if (index == list.Count)
//                {
//                    value = CreateObject(key);
//                    list.Add(value);
//                }
//                else
//                {
//                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range for the list.");
//                }
//            }
//            else
//            {
//                throw new ArgumentException("Container type or property name type is not supported or does not match.");
//            }

//            var sequence = new Sequence();
//            Action<object> updateContent = (updateData) =>
//            {
//                if (container is IList<object> list)
//                {
//                    list[index] = updateData;
//                }
//            };

//            void Component(RenderTreeBuilder builder)
//            {
//                var component = Configuration.GetElement(property);
//                if (component == null)
//                    return;
//                builder.AddAttribute(sequence++, "Form", this);
//                builder.AddAttribute(sequence++, "PropertyName", index.ToString());
//                builder.AddAttribute(sequence++, "PropertyType", key);
//                builder.AddAttribute(sequence++, "IsFirst", isFirst);
//                builder.AddAttribute(sequence++, "ValueChanged", updateContent);
//                builder.AddAttribute(sequence++, "Value", value);
//                builder.CloseComponent();
//            };
//            return FieldTemplate(new FieldTemplateContext(property, Component));
//        }

//        private async Task HandleSubmitAsync()
//        {
//            await OnValidSubmit.InvokeAsync(DataObject);
//        }
//    }
//}
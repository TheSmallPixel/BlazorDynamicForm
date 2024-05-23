using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Dynamic;
using BlazorDynamicForm.Entities;
using BlazorDynamicForm.Utility;

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
        public RenderFragment<FieldTemplateContext> FieldTemplate { get; set; }
        [Parameter]
        public RenderFragment SubmitTemplate { get; set; }

        [Inject]
        private DynamicFormConfiguration Configuration { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenRegion(GetHashCode());
            builder.OpenElement(0, "form");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(2, "onsubmit", HandleSubmitAsync);
            builder.AddAttribute(3, "novalidate");
            PopulateFormContent(builder);
            builder.AddContent(4, SubmitTemplate);
            builder.CloseElement();
            builder.CloseRegion();
        }

        private void PopulateFormContent(RenderTreeBuilder builder)
        {
            DataObject ??= new ExpandoObject();
            var dataDictionary = DataObject as IDictionary<string, object>;
            if (FormDefinition == null || dataDictionary == null)
                return;

            builder.OpenRegion(0);
            foreach (var frag in CreateComponents(FormDefinition.EntryType, FormDefinition.EntryType, dataDictionary))
            {
                builder.AddContent(1, frag);
            }
            builder.CloseRegion();
        }

        private object CreateObject(string key)
        {
            Console.WriteLine($"Creating {key}..");
            return FormDefinition.CreateObject(key, (option) =>
            {
                option.InitStringsEmpty = true;
                option.MaxRecursiveDepth = 10;
                option.CreateCollectionElement = false;
                option.CreateDictionaryElement = false;
                option.CreateObjectElement = false;
            });
        }

        //this can create Primitive, Array, Objects-
        private List<RenderFragment> CreateComponents(string key, string propertyName, IDictionary<string, object> data)
        {
            List<RenderFragment> fragments = new();
            if (!FormDefinition.Properties.TryGetValue(key, out var property))
                throw new InvalidOperationException($"The {key} is not in the Form Definition");

            if (!data.ContainsKey(propertyName))
                data[propertyName] = CreateObject(key);

            var sequence = new Sequence();
            var formComponentType = Configuration.GetElement(property);
            Action<object> updateContent = (updateData) => { data[propertyName] = updateData; };
            RenderFragment component = (builder) =>
            {
                Func<string, string, IDictionary<string, object>, List<RenderFragment>> opeFunc = CreateComponents;
                builder.OpenComponent(sequence++, formComponentType);
                builder.AddAttribute(sequence++, "ValueChanged", updateContent);
                builder.AddAttribute(sequence++, "Value", data[propertyName]);
                builder.AddAttribute(sequence++, "ChildBuilder", opeFunc);
                builder.AddAttribute(sequence++, "IsFirst", false);
                builder.AddAttribute(sequence++, "FormProperty", property);
                builder.AddAttribute(sequence++, "Formkey", key);
                builder.CloseComponent();
            };
            fragments.Add(FieldTemplate(new FieldTemplateContext(property, component)));
            return fragments;
        }

        private async Task HandleSubmitAsync()
        {
            await OnValidSubmit.InvokeAsync(DataObject);
        }
    }

    public class FieldTemplateContext
    {
        public FieldTemplateContext(FormProperty formProperty, RenderFragment dynamicComponent)
        {
            DynamicComponent = dynamicComponent;
            FormProperty = formProperty;
        }
        public RenderFragment DynamicComponent { get; private set; }
        public FormProperty FormProperty { get; private set; }
    }
}
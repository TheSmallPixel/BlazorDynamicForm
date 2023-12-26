namespace BlazorDynamicForm.Entities
{
    public class DynamicFormConfiguration
    {
        public Dictionary<string, DynamicFormElement> FormComponents = new();
        public RenderDynamicValidationMsgFragment FormErrorMsg { get; set; }
    }
}

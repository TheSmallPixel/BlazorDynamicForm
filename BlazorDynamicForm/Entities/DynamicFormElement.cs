namespace BlazorDynamicForm.Entities;

public class DynamicFormElement
{
    //public string? CustomDataAnnotationType { get; set; }
    public Type ComponentType { get; set; }
    public Type ComponentValueType { get; set; }
    //public Type FormComponentBase { get; set; }
    //public RenderDynamicContentFragment? DynamicContentFragment { get; set; }
}
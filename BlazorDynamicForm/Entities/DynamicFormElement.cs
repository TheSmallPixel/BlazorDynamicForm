using BlazorDynamicForm.Attributes.Display;

namespace BlazorDynamicForm.Entities;

public class DynamicFormElement
{
    public DataTypeAttribute.FormDatatype DataAnnotationType { get; set; }
    public string? CustomDataAnnotationType { get; set; }
    public Type ComponentType { get; set; }
    public Type ComponentValueType { get; set; }
    public RenderDynamicFragment? DynamicFragment { get; set; }
    public RenderDynamicContentFragment? DynamicContentFragment { get; set; }
}
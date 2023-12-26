using BlazorDynamicForm.Attributes.Display;
using BlazorDynamicForm.Attributes.Validation;

namespace BlazorDynamicForm.Entities;

public class FormProperty
{
    public string TypeName { get; set; }
    public string? TypeFullName { get; set; }
    public string? PropertyName { get; set; }
    public FormPropertyType PropertyType { get; set; }
    public List<DisplayForm> DisplayRules { get; set; } = new();
    public List<ValidationRule> ValidationRules { get; set; } = new();
    public List<CustomFormAttribute> CustomFormAttributes { get; set; } = new();
    public Dictionary<string, string> Structure { get; set; } = new();
}
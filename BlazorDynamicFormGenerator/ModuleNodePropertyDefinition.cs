using System.ComponentModel.DataAnnotations;

namespace BlazorDynamicFormGenerator;

public class ModuleNodePropertyDefinition
{
    public string Name { get; set; }
    public string? DisplayName { get; set; }
    public DataType? DataType { get; set; }
    public string? CustomDataType { get; set; }
    public string? Css { get; set; } = "form-control";
    public object? DefaultValue { get; set; }
    public bool ReadOnly { get; set; } = false;
    public List<ValidationAttribute> ValidationRules { get; set; } = new();
    public string[]? SelectionStrings { get; set; }
}
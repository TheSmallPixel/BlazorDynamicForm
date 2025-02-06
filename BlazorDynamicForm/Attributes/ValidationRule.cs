using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public abstract class ValidationRule : Attribute
{
    public ValidationRule()
    {
    }

    public ValidationRule(string propertyName)
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }

    public virtual bool IsValid(SchemeModel map, object? value)
    {
        return true;
    }

    public virtual string FormatErrorMessage(string name)
    {
        return string.Empty;
    }
}
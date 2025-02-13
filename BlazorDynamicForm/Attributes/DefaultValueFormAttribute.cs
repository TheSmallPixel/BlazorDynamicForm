using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public class DefaultValueFormAttribute : AttributeScheme
{
    public DefaultValueFormAttribute(object defaultValue)
    {
        DefaultValue = defaultValue;
    }

    public object DefaultValue { get; set; }
}
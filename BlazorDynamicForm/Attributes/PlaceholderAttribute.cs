using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public class PlaceholderAttribute : AttributeScheme
{
    public PlaceholderAttribute(string placeholder)
    {
        Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
}
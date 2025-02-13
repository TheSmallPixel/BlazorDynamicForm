using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public class DisplayNameFormAttribute : AttributeScheme
{
    public DisplayNameFormAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
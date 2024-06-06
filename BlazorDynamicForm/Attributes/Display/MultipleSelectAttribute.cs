using BlazorDynamicForm.Entities;

namespace BlazorDynamicForm.Attributes.Display;

public class MultipleSelectAttribute : DynamicRendererComponent
{
    public string[] Options { get; set; }
    public MultipleSelectAttribute(){}

    public MultipleSelectAttribute(params string[] options)
    {
        Options = options;
    }
}
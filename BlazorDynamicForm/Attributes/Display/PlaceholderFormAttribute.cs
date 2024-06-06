namespace BlazorDynamicForm.Attributes.Display;

public class PlaceholderFormAttribute : DisplayFormAttribute
{
    public PlaceholderFormAttribute(string placeholder)
    {
        Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
}
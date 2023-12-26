namespace BlazorDynamicForm.Attributes.Display;

public class PlaceholderFormAttribute : DisplayForm
{
    public PlaceholderFormAttribute(string placeholder)
    {
        Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
}
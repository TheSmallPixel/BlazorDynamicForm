namespace BlazorDynamicForm.Attributes.Display;

public class PlaceholderForm : DisplayForm
{
    public PlaceholderForm(string placeholder)
    {
        Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
}
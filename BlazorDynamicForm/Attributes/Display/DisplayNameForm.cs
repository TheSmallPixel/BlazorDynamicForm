namespace BlazorDynamicForm.Attributes.Display;

public class DisplayNameForm : DisplayForm
{
    public DisplayNameForm(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
namespace BlazorDynamicForm.Attributes.Display;

public class DisplayNameFormAttribute : DisplayFormAttribute
{
    public DisplayNameFormAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
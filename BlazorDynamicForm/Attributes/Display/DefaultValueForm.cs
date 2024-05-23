namespace BlazorDynamicForm.Attributes.Display;

public class DefaultValueForm : DisplayForm
{
    public DefaultValueForm(object defaultValue)
    {
        DefaultValue = defaultValue;
    }

    public object DefaultValue { get; set; }
}
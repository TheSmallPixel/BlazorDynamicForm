namespace BlazorDynamicForm.Attributes.Display;

public class DefaultValueAttribute : DisplayForm
{
    public DefaultValueAttribute(object defaultValue)
    {
        DefaultValue = defaultValue;
    }

    public object DefaultValue { get; set; }
}
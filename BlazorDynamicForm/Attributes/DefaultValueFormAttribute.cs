namespace BlazorDynamicForm.Attributes;

public class DefaultValueFormAttribute : DisplayFormAttribute
{
    public DefaultValueFormAttribute(object defaultValue)
    {
        DefaultValue = defaultValue;
    }

    public object DefaultValue { get; set; }
}
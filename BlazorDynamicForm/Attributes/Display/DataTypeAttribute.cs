namespace BlazorDynamicForm.Attributes.Display;

public class DataTypeAttribute : DisplayForm
{
    public enum FormDatatype
    {
        String,
        Integer,
        Float,
        MultilineString,
        Dictionary,
        Object,
        Custom,
        List,
        DatePicker,
        Boolean,
        MultiSelect
    }

    public DataTypeAttribute(FormDatatype type)
    {
        Type = type;
    }

    public FormDatatype Type { get; set; }
}
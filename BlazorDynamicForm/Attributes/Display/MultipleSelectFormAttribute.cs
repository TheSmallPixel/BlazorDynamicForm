namespace BlazorDynamicForm.Attributes.Display;

public class MultipleSelectFormAttribute : DataTypeAttribute
{
    public MultipleSelectFormAttribute(string[] options) : base(FormDatatype.MultiSelect)
    {
        Options = options;
    }

    public string[] Options { get; set; }
}
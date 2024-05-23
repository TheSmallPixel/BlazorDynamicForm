namespace BlazorDynamicForm.Attributes.Display;

public class MultipleSelectForm : DataTypeAttribute
{
    public MultipleSelectForm(string[] options) : base(FormDatatype.MultiSelect)
    {
        Options = options;
    }

    public string[] Options { get; set; }
}
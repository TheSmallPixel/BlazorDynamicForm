namespace BlazorDynamicForm.Attributes.Display;

public class FormDictionaryAttribute : DisplayForm
{
    public FormDictionaryAttribute(string indexName = "Key")
    {
        Index = indexName;
    }

    public string Index { get; set; }
}
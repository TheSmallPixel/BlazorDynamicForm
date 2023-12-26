namespace BlazorDynamicForm.Entities;

public class FormMap
{
    public Dictionary<string, FormProperty> Properties { get; set; } = new();
    public string EntryType { get; set; }
}
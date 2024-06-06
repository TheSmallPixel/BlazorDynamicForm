namespace BlazorDynamicForm;

public record ObjectGeneratorOptions
{
    public int MaxRecursiveDepth { get; set; } = 10;
    public bool InitStringsEmpty { get; set; } = true;

    public bool CreateCollectionElement { get; set; } = false;
    public bool CreateDictionaryElement { get; set; } = false;
    public bool CreateObjectElement { get; set; } = false;
}
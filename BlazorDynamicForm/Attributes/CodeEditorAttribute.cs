using BlazorDynamicForm.Core;

namespace BlazorDynamicForm.Attributes;

/// <summary>
/// Monaco: <see cref="https://microsoft.github.io/monaco-editor/"/> 
/// </summary>
public class CodeEditorAttribute : DynamicRendererComponent
{
    public string Language { get; set; }
    public string Theme { get; set; }
    public string Example { get; set; }


}
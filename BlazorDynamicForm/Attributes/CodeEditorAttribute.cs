using BlazorDynamicForm.Core;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

/// <summary>
/// Monaco: <see cref="https://microsoft.github.io/monaco-editor/"/> 
/// </summary>
public class CodeEditorAttribute : DynamicRendererComponent
{
    public CodeEditorAttribute(string language)
    {
        Language = language;
    }

    public CodeEditorAttribute(){}
    public string Language { get; set; }
    public string Theme { get; set; }
    public string Example { get; set; }


}
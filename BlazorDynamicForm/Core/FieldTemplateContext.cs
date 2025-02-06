using Microsoft.AspNetCore.Components;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Core;

public class FieldTemplateContext
{
    public FieldTemplateContext(SchemeProperty schemeProperty, RenderFragment dynamicComponent)
    {
        DynamicComponent = dynamicComponent;
        SchemeProperty = schemeProperty;
    }
    public RenderFragment DynamicComponent { get; private set; }
    public SchemeProperty SchemeProperty { get; private set; }
}
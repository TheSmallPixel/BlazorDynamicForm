using BlazorDynamicForm.Entities;
using Microsoft.AspNetCore.Components;
using TypeAnnotationParser;

namespace BlazorDynamicForm;

public class FieldTemplateContext
{
    public FieldTemplateContext(TypeAnnotationProperty typeAnnotationProperty, RenderFragment dynamicComponent)
    {
        DynamicComponent = dynamicComponent;
        TypeAnnotationProperty = typeAnnotationProperty;
    }
    public RenderFragment DynamicComponent { get; private set; }
    public TypeAnnotationProperty TypeAnnotationProperty { get; private set; }
}
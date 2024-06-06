using BlazorDynamicForm.Utility;
using Microsoft.AspNetCore.Components.Rendering;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Entities;

public delegate void RenderDynamicFragment(RenderTreeBuilder builder, Sequence sequence, TypeAnnotationProperty attribute, object data, object? eventCallback);
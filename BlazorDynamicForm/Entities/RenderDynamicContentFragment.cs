using BlazorDynamicForm.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Entities;

public delegate void RenderDynamicContentFragment(RenderTreeBuilder builder, TypeAnnotationModel map, string elementKey, Sequence sequence, TypeAnnotationProperty attribute, Func<string, object, List<RenderFragment>> child, object data, object? eventCallback);
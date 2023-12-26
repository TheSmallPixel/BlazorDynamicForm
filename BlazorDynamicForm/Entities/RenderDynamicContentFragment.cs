using BlazorDynamicForm.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDynamicForm.Entities;

public delegate void RenderDynamicContentFragment(RenderTreeBuilder builder, FormMap map, string elementKey, Sequence sequence, FormProperty attribute, Func<string, object, List<RenderFragment>> child, object data, object? eventCallback);
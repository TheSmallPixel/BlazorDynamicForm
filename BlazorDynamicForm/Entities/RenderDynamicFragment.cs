using BlazorDynamicForm.Utility;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDynamicForm.Entities;

public delegate void RenderDynamicFragment(RenderTreeBuilder builder, Sequence sequence, FormProperty attribute, object data, object? eventCallback);
using BlazorDynamicForm.Utility;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorDynamicForm.Entities;

public delegate void RenderDynamicValidationMsgFragment(RenderTreeBuilder builder, Sequence sequence, object value, string name, bool error);
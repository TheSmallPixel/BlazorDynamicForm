using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Core
{
	public abstract class FormComponentBase : ComponentBase
	{
		[Parameter] public SchemeModel? SchemeModel { get; set; }
		[Parameter] public SchemeProperty? SchemeProperty { get; set; }
		[Parameter] public string? PropertyName { get; set; }
		[Parameter] public string? PropertyType { get; set; }
		[Parameter] public bool IsFirst { get; set; }

		private object? _value;
		[Parameter] public EventCallback<object?> ValueChanged { get; set; }

		[Parameter]
		public object? Value
		{
			get => _value;
			set
			{
				if (!Equals(_value, value))
				{
					_value = value;
					ValueChanged.InvokeAsync(value);
				}
			}
		}
	}

}

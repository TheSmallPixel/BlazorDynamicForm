using Microsoft.AspNetCore.Components;

namespace BlazorDynamicForm.Core
{
    public abstract class FormComponentBase : ComponentBase
    {
        [Parameter]
        public DynamicForm Form { get; set; }
        [Parameter]
        public string PropertyName { get; set; }
        [Parameter]
        public string PropertyType { get; set; }
        [Parameter]
        public bool IsFirst { get; set; }
        [Parameter]
        public Action<object> ValueChanged { get; set; }
        [Parameter]
        public object Value
        {
            get => _innerValue;
            set
            {
                _innerValue = value;
                ValueChanged(value);

            }
        }

        private object _innerValue;
    }
}

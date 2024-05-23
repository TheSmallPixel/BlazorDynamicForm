using BlazorDynamicForm.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorDynamicForm.Components
{
    public abstract class FormComponentBase : ComponentBase
    {                                   
        [Parameter]
        public string Formkey { get; set; }

        [Parameter]
        public FormProperty FormProperty { get; set; }

        private object InnerValue;
        [Parameter]
        public object Value
        {
            get
            {
                return InnerValue;
            }
            set
            {
                InnerValue = value;
                ValueChanged(value);

            }
        }

        [Parameter]
        public bool IsFirst { get; set; }

        [Parameter]
        public Func<string, string, IDictionary<string, object>, List<RenderFragment>> ChildBuilder { get; set; }

        [Parameter]
        public Action<object> ValueChanged { get; set; }

    }
}

using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes
{
    public class RangeAttribute : AttributeScheme
    {
        public object Min { get; }
        public object Max { get; }

        public RangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public RangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public RangeAttribute(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }

        public RangeAttribute() {}

    }
}



using System.ComponentModel.DataAnnotations;

namespace BlazorDynamicFormGenerator
{
    public class ModuleNodeDefinition
    {
        public List<ModuleNodePropertyDefinition> PropertyDefinitions { get; set; } = new();


    }
    public class FormVar
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LinkedAttribute : ValidationAttribute
    {
        public string FilterType { get; set; }

        public LinkedAttribute() {}


        public LinkedAttribute(Type objectType)
        {
            FilterType = objectType.FullName;
            //DataType = DataType.Custom;
        }


        public override bool IsValid(object value)
        {
            var s = value as string;
            if (s == null)
                return false;

            if (string.IsNullOrWhiteSpace(s))
                return false;              

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReadOnlyAttribute : Attribute
    {
        
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorDynamicFormGenerator;

public static class ModuleNodePropertyDefinitionExtensions
{
    public static ModuleNodeDefinition GetDefinition<T>()
    {
        var definition = new ModuleNodeDefinition();
        var proList = typeof(T).GetProperties();
        foreach (var prp in proList)
        {
            var attrList = (DataTypeAttribute)prp.GetCustomAttributes(typeof(DataTypeAttribute), false).First();
            var displayLabel = (DisplayAttribute?)prp.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            var readOnly = ((ReadOnlyAttribute?)prp.GetCustomAttributes(typeof(ReadOnlyAttribute), false).FirstOrDefault() != null);
            var defaultValue = (DefaultValueAttribute?)prp.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault();
            var validationAttributes = prp.GetCustomAttributes(typeof(ValidationAttribute), false)
                .Cast<ValidationAttribute>()
                .ToList();

            var servicePropDef = new ModuleNodePropertyDefinition()
            {
                Name = prp.Name,
                DisplayName = displayLabel?.Name,
                DataType = attrList?.DataType,
                CustomDataType = attrList?.CustomDataType,
                ValidationRules = validationAttributes,
                ReadOnly = readOnly,
                DefaultValue = defaultValue?.Value
            };

            definition.PropertyDefinitions.Add(servicePropDef);

        }
        return definition;
    }
}
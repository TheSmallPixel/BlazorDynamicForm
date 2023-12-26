using System.Collections;
using BlazorDynamicForm.Entities;

namespace BlazorDynamicForm.Attributes.Validation;

public class RequiredRule : ValidationRule
{
    public override bool IsValid(FormMap map, object? value)
    {
        if (value == null)
        {
            return false;
        }

        switch (value)
        {
            case string stringValue:
                return !string.IsNullOrWhiteSpace(stringValue);
            case Array arrayValue:
                return arrayValue.Length > 0;
            case IDictionary dictionaryValue:
                return dictionaryValue.Count > 0;
            default:
                return true;
        }
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} is required.";
    }
}
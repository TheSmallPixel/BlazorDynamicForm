using System.Text;

namespace BlazorDynamicForm.Utility;

public static class StringUtils
{
    public static string AddSpacesToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var result = new StringBuilder();
        result.Append(input[0]);

        for (var i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append(' ');
            }

            result.Append(input[i]);
        }

        return result.ToString();
    }
}
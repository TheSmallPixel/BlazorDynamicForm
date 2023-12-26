using BlazorDynamicForm.Attributes.Display;
using BlazorDynamicForm.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDynamicForm.Utility;

public static class Loader
{
    public static DynamicFormConfiguration AddBlazorDynamicForm(this IServiceCollection services)
    {
        var config = new DynamicFormConfiguration();
        services.AddSingleton(config);
        return config;
    }

    public static DynamicFormConfiguration Add<C, T>(this DynamicFormConfiguration configuration, DataTypeAttribute.FormDatatype dataType,
        RenderDynamicFragment fragment = null)
    {
        configuration.FormComponents.Add(dataType.ToString(), new DynamicFormElement()
        {
            DataAnnotationType = dataType,
            ComponentType = typeof(C),
            ComponentValueType = typeof(T),
            DynamicFragment = fragment,
            CustomDataAnnotationType = null
        });

        return configuration;
    }
    public static DynamicFormConfiguration AddContainer<C, T>(this DynamicFormConfiguration configuration, DataTypeAttribute.FormDatatype dataType,
        RenderDynamicContentFragment fragment = null)
    {
        configuration.FormComponents.Add(dataType.ToString(), new DynamicFormElement()
        {
            DataAnnotationType = dataType,
            ComponentType = typeof(C),
            ComponentValueType = typeof(T),
            DynamicContentFragment = fragment,
            CustomDataAnnotationType = null
        });

        return configuration;
    }

    public static DynamicFormConfiguration AddError(this DynamicFormConfiguration configuration, RenderDynamicValidationMsgFragment fragment)
    {
        configuration.FormErrorMsg = fragment;

        return configuration;
    }

    public static void DefaultDynamicForm(this DynamicFormConfiguration config)
    {
        config
            .Add<InputText, string>(DataTypeAttribute.FormDatatype.String)
            //.Add<InputText, string>(DataType.EmailAddress)
            //.Add<InputText, string>(DataType.PhoneNumber)
            //.Add<InputTextArea, string>(DataType.MultilineText)
            //.Add<InputDate<DateTime>, DateTime?>(DataType.DateTime)
            //.Add<InputDate<DateTimeOffset>, DateTimeOffset>(DataType.Duration)
            .AddError((builder, sequence, value, name, error) =>
            {
                builder.AddAttribute(sequence++, "Value", value);
                builder.AddAttribute(sequence++, "PlaceHolder", name);
                builder.AddAttribute(sequence++, "class", error ? "is-invalid" : "is-valid");
            });
    }

}
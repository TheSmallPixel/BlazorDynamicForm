
using System.ComponentModel.DataAnnotations;  
using BlazorDynamicFormGenerator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDynamicFormDataAnnotation
{
    public static class Loader
    {
        public static DynamicFormConfiguration AddBlazorDynamicForm(this IServiceCollection services)
        {
            var config = new DynamicFormConfiguration();
            services.AddSingleton(config);
            return config;
        }

        public static DynamicFormConfiguration Add<C, T>(this DynamicFormConfiguration configuration, DataType dataType,
            RenderDynamicFragment fragment = null)
        {
            configuration.FormComponents.Add(dataType.ToString(), new DynamicFormElement()
            {
                DataAnnotationType = dataType,
                DataType = typeof(T),
                ComponentType = typeof(C),
                DynamicFragment = fragment,
                CustomDataAnnotationType = null
            });

            return configuration;
        }

        public static DynamicFormConfiguration AddDefault<C, T>(this DynamicFormConfiguration configuration,
            RenderDynamicFragment fragment = null)
        {
            configuration.FormComponents.TryAdd("Default", new DynamicFormElement()
            {
                DataAnnotationType = DataType.Custom,
                DataType = typeof(T),
                ComponentType = typeof(C),
                DynamicFragment = fragment,
                CustomDataAnnotationType = null
            });

            return configuration;
        }

        public static DynamicFormConfiguration AddCustom<C, T>(this DynamicFormConfiguration configuration,
            string customDataType, RenderDynamicFragment fragment = null)
        {
            configuration.FormComponents.Add(customDataType, new DynamicFormElement()
            {
                DataAnnotationType = DataType.Custom,
                CustomDataAnnotationType = customDataType,
                DataType = typeof(T),
                ComponentType = typeof(C),
                DynamicFragment = fragment

            });

            return configuration;
        }

        public static DynamicFormConfiguration AddDataProvider(this DynamicFormConfiguration configuration, FormDataSource dataSource)
        {
            configuration.DataSource = dataSource;
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
                .Add<InputText, string>(DataType.Text)
                .Add<InputText, string>(DataType.EmailAddress)
                .Add<InputText, string>(DataType.PhoneNumber)
                .Add<InputTextArea, string>(DataType.MultilineText)
                .Add<InputDate<DateTime>, DateTime?>(DataType.DateTime)
                .Add<InputDate<DateTimeOffset>, DateTimeOffset>(DataType.Duration)
                .AddError((builder, sequence, value, name, error) =>
                {
                    builder.AddAttribute(sequence++, "Value", value);
                    builder.AddAttribute(sequence++, "PlaceHolder", name);
                    builder.AddAttribute(sequence++, "class", error ? "is-invalid" : "is-valid");
                });
        }

    }

    public class DynamicFormElement
    {
        public DataType DataAnnotationType { get; set; }
        public string? CustomDataAnnotationType { get; set; }
        public Type ComponentType { get; set; }
        public Type DataType { get; set; }
        public RenderDynamicFragment? DynamicFragment { get; set; }

    }

    public class FormVar
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public delegate IList<FormVar> FormDataSource(LinkedAttribute attribute, string varName);
    public delegate void RenderDynamicFragment(RenderTreeBuilder builder, Sequence sequence, ModuleNodePropertyDefinition attribute);
    public delegate void RenderDynamicValidationMsgFragment(RenderTreeBuilder builder, Sequence sequence, object value, string name, bool error);

    public class DynamicFormFragment
    {
        public RenderFragment RenderFragment { get; set; }
    }

    public class DynamicFormConfiguration
    {
        public Dictionary<string, DynamicFormElement> FormComponents = new();
        public RenderDynamicValidationMsgFragment FormErrorMsg { get; set; }
        public FormDataSource DataSource { get; set; }
    }


    public class Sequence
    {
        public int Value { get; set; }

        public static implicit operator int(Sequence i) => i.Value;

        public static Sequence operator ++(Sequence left)
        {
            left.Value++;
            return left;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}

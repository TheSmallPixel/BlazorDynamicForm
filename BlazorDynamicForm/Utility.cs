using BlazorDynamicForm.Attributes;
using BlazorDynamicForm.Components;
using BlazorDynamicForm.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TypeAnnotationParser;
using CodeEditorAttribute = BlazorDynamicForm.Attributes.CodeEditorAttribute;

namespace BlazorDynamicForm;

public static class Utility
{
    public static List<Type> DefaultComponents = new List<Type>
    {
             typeof(MultipleSelectAttribute),
             typeof(TextAreaAttribute),
             typeof(CodeEditorAttribute),

    };

    public static IServiceCollection AddBlazorDynamicForm(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DynamicFormConfiguration>>();
            var config = new DynamicFormConfiguration(logger);

            config.AddRenderer<EnumComponent>(PropertyType.Enum);
            config.AddRenderer<DecimalComponent>(PropertyType.Decimal);
            config.AddRenderer<FloatComponent>(PropertyType.Float);
            config.AddRenderer<IntComponent>(PropertyType.Integer);
            config.AddRenderer<StringComponent>(PropertyType.String);

            config.AddRenderer<ObjectComponent>(PropertyType.Object);
            config.AddRenderer<ListComponent>(PropertyType.Array);
            config.AddRenderer<DictionaryComponent>(PropertyType.Dictionary);

            config.AddCustomRenderer<FileComponent>(PropertyType.File);

            config.AddCustomAttributeRenderer<MultipleSelectAttribute, MultipleOptionsComponent>();
            config.AddCustomAttributeRenderer<CodeEditorAttribute, CodeEditorComponent>();
            config.AddCustomAttributeRenderer<TextAreaAttribute, TextAreaComponent>();

            return config;
        });

        return services;
    }

}
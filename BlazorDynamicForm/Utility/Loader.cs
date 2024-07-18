using BlazorDynamicForm.Attributes;
using BlazorDynamicForm.Components;
using BlazorDynamicForm.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorDynamicForm.Utility;

public static class Loader
{
    public static IServiceCollection AddBlazorDynamicForm(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<DynamicFormConfiguration>>();
            var config = new DynamicFormConfiguration(logger);

            config.AddPrimitive<float, FloatComponent>();
            config.AddPrimitive<int, IntComponent>();
            config.AddPrimitive<string, StringComponent>();

            config.AddObjectRenderer<ObjectComponent>();
            config.AddCollectionRenderer<ListComponent>();
            config.AddDictionaryRenderer<DictionaryComponent>();

            config.AddCustomRenderer<FileData, FileComponent>();

            config.AddCustomAttributeRenderer<MultipleSelectAttribute, MultipleOptionsComponent>();
            config.AddCustomAttributeRenderer<CodeEditorAttribute, CodeEditorComponent>();
            config.AddCustomAttributeRenderer<TextAreaAttribute, TextAreaComponent>();

            return config;
        });

        return services;
    }

}
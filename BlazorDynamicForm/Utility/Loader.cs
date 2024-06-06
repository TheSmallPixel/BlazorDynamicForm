using System.Collections;
using BlazorDynamicForm.Attributes.Display;
using BlazorDynamicForm.Components.Defaults;
using BlazorDynamicForm.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDynamicForm.Utility;

public static class Loader
{
    public static DynamicFormConfiguration AddBlazorDynamicForm(this IServiceCollection services)
    {
        var config = new DynamicFormConfiguration();
        services.AddSingleton(config);
        //TODO: add special componnet config.AddCustomRenderer<CodeEditor, CodeEditor>();

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
        //config.AddCustomRenderer<FileAttribute, >();
        return config;
    }

}
﻿using Microsoft.Extensions.Logging;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Core
{
    public class DynamicFormConfiguration(ILogger<DynamicFormConfiguration> logger)
    {

        public Dictionary<PropertyType, Type> RendererMappings { get; private set; } = new();

        public Dictionary<Type, Type> CustomAttributeRenderer { get; private set; } = new();

        public Dictionary<PropertyType, Type> CustomRenderer { get; private set; } = new();

        public void AddRenderer<R>(PropertyType type) where R : FormComponentBase
        {
            RendererMappings[type] = typeof(R);
        }

        public void AddCustomAttributeRenderer<T, R>() where R : FormComponentBase where T : DynamicRendererComponent
        {
            CustomAttributeRenderer[typeof(T)] = typeof(R);
        }

        public void AddCustomRenderer<R>(PropertyType type) where R : FormComponentBase
        {
            CustomRenderer[type] = typeof(R);
        }

        public SchemeProperty? ResolveReference(SchemeModel model, SchemeProperty property)
        {
            if (string.IsNullOrEmpty(property.Ref))
            {
                return property;
            }
            if (model.References.TryGetValue(property.Ref, out var prop))
            {
                return prop;
            }
            return null;
        }

        public Type? GetElement(SchemeModel model, SchemeProperty property)
        {

            if (property.Type is null && string.IsNullOrEmpty(property.Ref))
            {
                return null;             //exeption		invalid
            }

            if (property.Type is null && !string.IsNullOrEmpty(property.Ref))
            {
                if (model.References.TryGetValue(property.Ref, out var prop))
                {
                    return GetElement(model, prop);
                }
            }
            else
            {
                var customRenderer = property.Attributes?.OfType<DynamicRendererComponent>().FirstOrDefault();
                if (customRenderer != null && CustomAttributeRenderer.TryGetValue(customRenderer.GetType(), out var component))
                {
                    return component;
                }

                if (CustomRenderer.TryGetValue(property.Type.Value, out var customElement))
                {
                    return customElement;
                }
                if (RendererMappings.TryGetValue(property.Type.Value, out var element))
                {
                    return element;
                }
            }

            return null;

        }
    }
}

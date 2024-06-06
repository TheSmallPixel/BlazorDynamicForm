using BlazorDynamicForm.Components;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Entities
{
    public class DynamicFormConfiguration
    {
        public Type ObjectRenderer { get; private set; }
        public Type CollectionRenderer { get; private set; }
        public Type DictionaryRenderer { get; private set; }

        public Dictionary<Type, Type> PrimitiveRenderer { get; private set; } = new();
      
        public Dictionary<Type, Type> CustomAttributeRenderer { get; private set; } = new();

        public Dictionary<string, Type> CustomRenderer { get; private set; } = new();

        public void AddObjectRenderer<T>() where T : FormComponentBase
        {
            ObjectRenderer = typeof(T);
        }
        public void AddCollectionRenderer<T>() where T : FormComponentBase
        {
            CollectionRenderer = typeof(T);
        }
        public void AddDictionaryRenderer<T>() where T : FormComponentBase
        {
            DictionaryRenderer = typeof(T);
        }

        public void AddPrimitive<T, R>() where R : FormComponentBase
        {
            PrimitiveRenderer[typeof(T)] = typeof(R);
        }

        public void AddCustomAttributeRenderer<T, R>() where R : FormComponentBase where T : DynamicRendererComponent
        {
            CustomAttributeRenderer[typeof(T)] = typeof(R);
        }

        public void AddCustomRenderer<T, R>() where R : FormComponentBase
        {
            CustomRenderer[typeof(T).FullName] = typeof(R);
        }


        public Type GetElement(TypeAnnotationProperty property)
        {
            var customRenderer = property.Attributes?.OfType<DynamicRendererComponent>().FirstOrDefault();

            if (customRenderer != null)
            {
                if (CustomAttributeRenderer.TryGetValue(customRenderer.GetType(), out var component))
                {
                    return component;
                }
            }

            if (!string.IsNullOrWhiteSpace(property.Type) && CustomRenderer.TryGetValue(property.Type, out var customElement))
            {
                return customElement;
            }

            switch (property.PropertyType)
            {
                case PropertyType.Primitive:
                    if (PrimitiveRenderer.TryGetValue(Type.GetType(property.Type), out var element))
                    {
                        return element;
                    }
                    throw new InvalidOperationException("Invalid property found");
                case PropertyType.Object:
                    return ObjectRenderer;
                case PropertyType.Collection:
                    return CollectionRenderer;
                case PropertyType.Dictionary:
                    return DictionaryRenderer;
                default:
                    throw new InvalidOperationException("Invalid property found");
            }
        }
    }
}

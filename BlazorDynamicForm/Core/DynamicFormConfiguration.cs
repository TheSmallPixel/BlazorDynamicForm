using Microsoft.Extensions.Logging;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Core
{
	public class DynamicFormConfiguration
	{
		public Type ObjectRenderer { get; private set; }
		public Type CollectionRenderer { get; private set; }
		public Type DictionaryRenderer { get; private set; }

		public Dictionary<PropertyType, Type> PrimitiveRenderer { get; private set; } = new();

		public Dictionary<Type, Type> CustomAttributeRenderer { get; private set; } = new();

		public Dictionary<PropertyType, Type> CustomRenderer { get; private set; } = new();

		private readonly ILogger<DynamicFormConfiguration> _logger;

		public DynamicFormConfiguration(ILogger<DynamicFormConfiguration> logger)
		{
			_logger = logger;
		}

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

		public void AddPrimitive<R>(PropertyType type) where R : FormComponentBase
		{
			PrimitiveRenderer[type] = typeof(R);
		}

		public void AddCustomAttributeRenderer<T, R>() where R : FormComponentBase where T : DynamicRendererComponent
		{
			CustomAttributeRenderer[typeof(T)] = typeof(R);
		}

		public void AddCustomRenderer<R>(PropertyType type) where R : FormComponentBase
		{
			CustomRenderer[type] = typeof(R);
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
				if (CustomAttributeRenderer.TryGetValue(customRenderer.GetType(), out var component))
				{
					return component;
				}

				if (CustomRenderer.TryGetValue(property.Type.Value, out var customElement))
				{
					return customElement;
				}
				if (PrimitiveRenderer.TryGetValue(property.Type.Value, out var element))
				{
					return element;
				}
			}

			return null;

		}
	}
}

using System.Collections;
using System.Reflection;

namespace TypeAnnotationParser;

public class TypeAnnotationParser(ParserConfiguration configuration)
{
	public SchemeModel Parse<T>()
	{
		var type = typeof(T);
		var form = new SchemeModel();
		AddFormProperty(form, form, type);
		return form;
	}

	public SchemeModel Parse(Type type)
	{
		var form = new SchemeModel();
		AddFormProperty(form, form, type);
		return form;
	}

	private string GetUniquePropertyKey(Type parentType)
	{
		return $"#{parentType.Name}";
	}

	private SchemeProperty AddFormProperty(SchemeModel scheme, SchemeProperty schemeProperty, Type propertyType, uint depth = 0, PropertyInfo? propertyInfo = null)
	{
		depth+=1;
		if (depth >= 20)
			return schemeProperty;
		schemeProperty.Type = DeterminePropertyType(propertyType);

		//if (schemeProperty is not { Type: PropertyType.Object })
		//	schemeProperty.Name = propertyInfo != null ? propertyInfo.Name : propertyType.Name;
		if (schemeProperty.Type is PropertyType.Object)
			schemeProperty.Name = propertyType.Name;
		AssignAttributesToProperty(propertyType, schemeProperty);
		if (propertyInfo != null)
			AssignAttributesToProperty(propertyInfo, schemeProperty);

		switch (schemeProperty.Type)
		{
			case PropertyType.Object:
				{
					foreach (var propInfo in propertyType.GetProperties())
					{
						var keyProp = GetUniquePropertyKey(propInfo.PropertyType);
						if (!scheme.References.TryGetValue(keyProp, out var objectPropertyScheme))
						{
							if (DeterminePropertyType(propInfo.PropertyType) is PropertyType.Object)
							{
								scheme.References.TryAdd(keyProp, new SchemeProperty(){Type = PropertyType.Object});
							}
							objectPropertyScheme = AddFormProperty(scheme, new SchemeProperty(), propInfo.PropertyType, depth, propInfo);
							if (objectPropertyScheme.Type is PropertyType.Object)
								scheme.References[keyProp] = objectPropertyScheme;
						}
						schemeProperty.Properties ??= new();
						//add reference
						if (objectPropertyScheme.Type is PropertyType.Object)
						{
							schemeProperty.Properties.Add(propInfo.Name, new SchemeProperty() { Ref = keyProp });
						}
						else
						{
							schemeProperty.Properties.Add(propInfo.Name, objectPropertyScheme);
						}
					}
					break;
				}
			case PropertyType.Array or PropertyType.Dictionary:
				{
					var elementTypes = GetBaseArrayType(propertyType);
					foreach (var indexType in elementTypes)
					{
						var keyProp = GetUniquePropertyKey(indexType);
						if (!scheme.References.TryGetValue(keyProp, out var objectPropertyScheme))
						{
							
							objectPropertyScheme = AddFormProperty(scheme, new SchemeProperty(), indexType, depth);
							if (objectPropertyScheme.Type is PropertyType.Object)
								scheme.References.Add(keyProp, objectPropertyScheme);
						}
						schemeProperty.Indices ??= new();
						if (objectPropertyScheme.Type is PropertyType.Object)
						{
							schemeProperty.Indices.Add(new SchemeProperty() { Ref = keyProp });
						}
						else
						{
							schemeProperty.Indices.Add(objectPropertyScheme);
						}
					}
					break;
				}
			default:
				break;
		}


		return schemeProperty;
	}

	private void AssignAttributesToProperty(MemberInfo property, SchemeProperty schemeProperty)
	{
		var attributes = property.GetCustomAttributes(typeof(AttributeScheme), true).Cast<AttributeScheme>().ToList();
		if (!attributes.Any()) return;
		schemeProperty.Attributes ??= [];
		schemeProperty.Attributes.AddRange(attributes);
	}

	public PropertyType DeterminePropertyType(Type type)
	{
		if (type == null)
			throw new ArgumentNullException(nameof(type));

		// Check for Enum first because enums are treated as value types.
		if (type.IsEnum)
			return PropertyType.Enum;

		// Handle the string type.
		if (type == typeof(string))
			return PropertyType.String;

		// Check for specific numeric types.
		if (type == typeof(float))
			return PropertyType.Float;

		if (type == typeof(double))
			return PropertyType.Double;

		// Check for various integer types.
		if (type == typeof(byte) || type == typeof(sbyte) ||
			type == typeof(short) || type == typeof(ushort) ||
			type == typeof(int) || type == typeof(uint) ||
			type == typeof(long) || type == typeof(ulong))
		{
			return PropertyType.Integer;
		}

		// Arrays and collections: 
		// Check if the type is an array or implements IList.
		if (type.IsArray || typeof(IList).IsAssignableFrom(type))
			return PropertyType.Array;

		// Check if the type implements IDictionary.
		if (typeof(IDictionary).IsAssignableFrom(type))
			return PropertyType.Dictionary;

		// Fallback to object for any other type.
		return PropertyType.Object;
	}

	private Type[] GetBaseArrayType(Type type)
	{
		if (type.IsArray)
		{
			return [type.GetElementType()!];
		}

		if (typeof(IEnumerable).IsAssignableFrom(type) || typeof(IDictionary).IsAssignableFrom(type))
		{
			if (type.IsGenericType)
			{
				return type.GetGenericArguments();
			}
			return Array.Empty<Type>();
		}
		return Array.Empty<Type>();
	}
}
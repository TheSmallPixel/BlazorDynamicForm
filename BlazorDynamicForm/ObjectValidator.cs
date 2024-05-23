﻿using BlazorDynamicForm.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDynamicForm
{
    public static class ObjectValidator
    {

        public static bool Validate(this FormMap definition, object propertyValue)
        {
            return Validate(definition, definition.EntryType, propertyValue);
        }
        public static bool Validate(this FormMap definition, string key, object propertyValue)
        {
            if (propertyValue == null) return false;
            var prop = definition.Properties[key];
            // Type dataType = data.GetType();
            switch (prop.PropertyType)
            {
                case FormPropertyType.Primitive:
                    if (propertyValue == null) return true; // Consider what should happen if null is not allowed
                    Type expectedType = Type.GetType(prop.Type);
                    return expectedType.IsInstanceOfType(propertyValue);
                case FormPropertyType.Object:
                    //var objectData = propertyValue as IDictionary<string, object>;
                    //if (objectData == null) return true;

                    if (propertyValue is IDictionary<string, object> propertyValueDic)
                    {
                        foreach (var property in prop.Properties)
                        {

                            if (propertyValueDic.ContainsKey(property.Key))
                            {
                                if (!Validate(definition, property.Value, propertyValueDic[property.Key]))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                //is not in the configuration
                                return false;
                            }
                        }
                    }
                    else
                    {
                        var propsInClass = propertyValue.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                        foreach (var property in prop.Properties)
                        {
                            var classValue = propsInClass.FirstOrDefault(x => x.Name == property.Key);
                            if (classValue != null)
                            {
                                if (!Validate(definition, property.Value, classValue.GetValue(propertyValue)))
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                //is not in the configuration
                                return false;
                            }
                        }
                    }

                    return true;
                case FormPropertyType.Collection:
                    var collectionData = propertyValue as IEnumerable;
                    if (collectionData == null) return true;
                    var keyType = prop.Properties.First().Value;
                    foreach (var property in collectionData)
                    {
                        if (!Validate(definition, keyType, property))
                        {
                            return false;
                        }
                    }
                    return true;
                case FormPropertyType.Dictionary:

                    var dicKey = prop.Properties.First().Value;
                    var dicValue = prop.Properties.Last().Value;
                    var dictionaryData = propertyValue as IDictionary<object, object>;
                    if (dictionaryData == null) return true;
                    foreach (var property in dictionaryData)
                    {
                        if (!Validate(definition, dicKey, property.Key))
                        {
                            return false;
                        }
                        if (!Validate(definition, dicValue, property.Value))
                        {
                            return false;
                        }
                    }


                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}

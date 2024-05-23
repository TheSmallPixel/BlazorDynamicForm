//using Syncfusion.Blazor.Calendars;
//using Syncfusion.Blazor.Inputs;           
//using Syncfusion.Blazor.DropDowns;
//using System.Dynamic;
//using BlazorDynamicForm.Attributes.Display;
//using BlazorDynamicForm.Entities;
//using BlazorDynamicForm.Utility;
//using Syncfusion.Blazor.Buttons;             

//namespace BlazorDynamicFormSyncfusion
//{

//    public static class SyncfusionIntegration
//    {
//        public static void SyncfusionForm(this DynamicFormConfiguration config)
//        {
//            config
//                .Add<SfTextBox,string>(DataTypeAttribute.FormDatatype.String, (builder, sequence, attribute, data, callback) =>
//                {
                   
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);

//                    builder.AddAttribute(sequence++, "Value", data);
//                    var placeholderAttribute = attribute.DisplayRules.OfType<PlaceholderForm>().FirstOrDefault();
//                    if (placeholderAttribute != null)
//                        builder.AddAttribute(sequence++, "Placeholder", placeholderAttribute.Placeholder);
//                    var readonlyFormAttributeAttribute = attribute.DisplayRules.OfType<ReadonlyFormAttribute>().FirstOrDefault();
//                    if (readonlyFormAttributeAttribute != null)
//                        builder.AddAttribute(sequence++, "Readonly", true);
                  
//                })
//                .Add<SfTextBox, string>(DataTypeAttribute.FormDatatype.MultilineString, (builder, sequence, attribute, data, callback) =>
//                {
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "Multiline", true);
//                    builder.AddAttribute(sequence++, "Value", data);
//                    var placeholderAttribute = attribute.DisplayRules.OfType<PlaceholderForm>().FirstOrDefault();
//                    if (placeholderAttribute != null)
//                        builder.AddAttribute(sequence++, "Placeholder", placeholderAttribute.Placeholder);
//                    //builder.AddAttribute(sequence++, "Readonly", attribute.ReadOnly);
              
//                })
//                .Add<SfTimePicker<DateTime?>, DateTime?>(DataTypeAttribute.FormDatatype.DatePicker, (builder, sequence, attribute, data, callback) =>
//                {
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "Value", data);
                  
//                })
//                .Add<SfSwitch<bool>, bool>(DataTypeAttribute.FormDatatype.Boolean, (builder, sequence, attribute, data, callback) =>
//                {
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "Value", data);
   
//                })
//                .Add<SfDropDownList<string, string>,string>(DataTypeAttribute.FormDatatype.MultiSelect, (builder, sequence, attribute, data, callback) =>
//                {
//                    builder.AddAttribute(sequence++, "ValueChanged", callback); 

//                    builder.AddAttribute(sequence++, "Value", data);
//                    var options = attribute.DisplayRules.OfType<MultipleSelectForm>().FirstOrDefault();
//                    if (options != null)
//                        builder.AddAttribute(sequence++, "DataSource", options.Options);


//                })
//                .AddContainer<ListSync, object>(DataTypeAttribute.FormDatatype.List, (builder, FormMap, key, sequence, attribute, childBuilder, data, callback) =>
//                {
//                    var container = data as List<ExpandoObject>;
//                    builder.AddAttribute(sequence++, "Value", container);
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "ChildBuilder", childBuilder);
//                    builder.AddAttribute(sequence++, "Formmap", FormMap);
//                    builder.AddAttribute(sequence++, "Formkey", key);
              
//                })
//                .AddContainer<DicSync, object>(DataTypeAttribute.FormDatatype.Dictionary, (builder, FormMap, key, sequence, attribute, childBuilder, data, callback) =>
//                {
//                    var container = data as List<ExpandoObject>;
//                    builder.AddAttribute(sequence++, "Value", container);
//                    builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "ChildBuilder", childBuilder);
//                    builder.AddAttribute(sequence++, "Formmap", FormMap);
//                    builder.AddAttribute(sequence++, "Formkey", key);
//                })
//                .AddContainer<ObjectSync, object>(DataTypeAttribute.FormDatatype.Object, (builder, FormMap, key, sequence, attribute, childBuilder, data, callback) =>
//                {
//                    var container = data as ExpandoObject;
//                    builder.AddAttribute(sequence++, "Value", container);
//                    // builder.AddAttribute(sequence++, "ValueChanged", callback);
//                    builder.AddAttribute(sequence++, "ChildBuilder", childBuilder);
//                    builder.AddAttribute(sequence++, "Formmap", FormMap);
//                    builder.AddAttribute(sequence++, "Formkey", key);
//                })
//                .AddError((builder, sequence, value, name, error) =>
//                {
//                    builder.AddAttribute(sequence++, "Value", value);
//                    builder.AddAttribute(sequence++, "PlaceHolder", name);
//                    builder.AddAttribute(sequence++, "class", error ? "is-invalid" : "is-valid");
//                });


//        }
//    }
//}
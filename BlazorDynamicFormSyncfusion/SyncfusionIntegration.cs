using BlazorDynamicFormDataAnnotation;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using BlazorDynamicFormGenerator;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using FormVar = BlazorDynamicFormDataAnnotation.FormVar;

namespace BlazorDynamicFormSyncfusion
{
    public static class SyncfusionIntegration
    {
        public static void SyncfusionForm(this DynamicFormConfiguration config)
        {
            config
                .Add<SfTextBox, string>(DataType.Text, (builder, sequence, attribute) =>
                {
                    builder.AddAttribute(sequence++, "Readonly", attribute.ReadOnly);
                })
                .Add<SfTextBox, string>(DataType.EmailAddress)
                .Add<SfTextBox, string>(DataType.PhoneNumber)
                .Add<SfTextBox, string>(DataType.MultilineText, (builder, sequence, attribute) =>
                {
                    builder.AddAttribute(sequence++, "Multiline", true);
                    builder.AddAttribute(sequence++, "Readonly", attribute.ReadOnly);
                })
                .Add<SfDatePicker<DateTime?>, DateTime?>(DataType.DateTime)
                .Add<SfNumericTextBox<decimal?>, decimal?>(DataType.Duration)
                .AddCustom<SfDropDownList<string, FormVar>, string>("DropdownList", (builder, sequence, attribute) =>
                {
                    var linked = attribute.ValidationRules.OfType<LinkedAttribute>().FirstOrDefault();
                    if (linked is not null)
                        builder.AddAttribute(sequence++, "DataSource", config.DataSource(linked, attribute.Name));
                    builder.AddAttribute(sequence++, "ChildContent", (RenderFragment)((builder2) =>
                    {
                        builder2.AddMarkupContent(0, "\r\n");
                        builder2.OpenComponent<DropDownListFieldSettings>(1);
                        builder2.AddAttribute(2, "Value", "Id");
                        builder2.AddAttribute(3, "Text", "Name");
                        builder2.CloseComponent();
                        builder2.AddMarkupContent(4, "\r\n");
                    }));
                }) 
                .AddError((builder, sequence, value, name, error) =>
                {
                    builder.AddAttribute(sequence++, "Value", value);
                    builder.AddAttribute(sequence++, "PlaceHolder", name);
                    builder.AddAttribute(sequence++, "class", error ? "is-invalid" : "is-valid");
                });
        }
    }
}
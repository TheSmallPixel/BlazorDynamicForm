# Blazor Dynamic Form

BlazorDynamicFormSyncfusion is a dynamic form generator built with C# for Blazor applications, with an integration of the Syncfusion Blazor UI library. It uses data annotation attributes to automatically build forms with validation included.

  - [Installation](#installation)
    - [.NET CLI](#net-cli)
    - [Packet Manager](#packet-manager)
  - [Packages Description](#packages-description)
  - [Form Usage](#form-usage)
  - [Data Providers](#data-providers)
  - [Create a Custom Integration](#create-a-custom-integration)
    - [Add Method](#add-method)
    - [AddCustom Method](#addcustom-method)
  - [Contributing](#contributing)
  - [License](#license)

## Features

- Built-in data annotations support for forms.
- Syncfusion Blazor components integration.
- Customization options for form fields.
- Automatic form generation and handling.

## Installation

.NET CLI
```bash
dotnet add package BlazorDynamicFormDataAnnotation --version 1.0.4
dotnet add package BlazorDynamicFormGenerator --version 1.0.4
dotnet add package BlazorDynamicFormSyncfusion --version 1.0.4
```

Packet Manager
```bash
Install-Package BlazorDynamicFormDataAnnotation -Version 1.0.4
Install-Package BlazorDynamicFormGenerator -Version 1.0.4
Install-Package BlazorDynamicFormSyncfusion -Version 1.0.4
```
# Packages Description

This project includes three essential packages that handle various aspects of dynamic form generation and management in Blazor. Here's a brief rundown of what each package offers:

1. **BlazorDynamicFormDataAnnotation**: This package is the heart of the form generation process in Blazor. It employs DataAnnotations for generating dynamic forms, taking the grunt work out of the process. Leveraging the framework's capabilities, it automates form creation based on your annotated model.

2. **BlazorDynamicFormGenerator**: This package acts as a blueprint generator. It dynamically produces class description templates, allowing the frontend to function independently of hardcoded class definitions. The generated templates cater to both fixed and fluid design requirements, offering you the flexibility to adapt to evolving needs.

3. **BlazorDynamicFormSyncfusion**: This extension package is a bridge between the Blazor Dynamic Form and Syncfusion Blazor components. It allows seamless integration of Syncfusion components into the generated forms, offering enhanced user interaction and richer data handling. Each component can be assigned to specific data types or attributes, promoting consistent and streamlined form structures.

Together, these packages serve as a robust toolset for handling form generation in Blazor applications, offering flexibility, ease of use, and integration with industry-leading component libraries.
## Form Usage

In your Blazor application, include the namespace for the dynamic form generator.

```razor
@using BlazorDynamicFormDataAnnotation
```

Here is an example of a dynamic form in a Razor component:

```razor
<DynamicForm DataDefinition="@definition" DataObject="@data" OnValidSubmit="@OnValidResult">
    <ValidationMessageTemplate Context="error">
        @error
    </ValidationMessageTemplate>
    <FieldTemplate Context="template">
        <div class="row mb-3">
            <label for="@template.PropertyInfo.Name" class="col-sm-2 col-form-label">@template.PropertyInfo.DisplayName</label>
            <div class="has-validation col-sm-10">
                <div class="col">
                    @template.DynamicComponent
                </div>
                @if (!template.IsValid)
                {
                    <div class="invalid-feedback d-block">
                        @template.ValidationComponent
                    </div>
                }
            </div>
        </div>
    </FieldTemplate>
    <SubmitTemplate>
        <div class="col-12">
            <button type="submit" class="btn btn-primary">Save</button>
        </div>
    </SubmitTemplate>
</DynamicForm>
```

Refer to the included `Test` class in the `OnInitialized()` method for an example of a model with data annotations.

```csharp
    public class Test
    {
        [Required, Display(Name = "First Name"), DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"), DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required, Display(Name = "Email Address"), DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "PhoneNumber"), DataType(DataType.PhoneNumber), Phone]
        public string PhoneNumber { get; set; }

        [Required, Display(Name = "Date of Birth"), DataType(DataType.DateTime)]
        public DateTime? DOB { get; set; }

        [Required, DataType(DataType.Duration), Display(Name = "Total Experience"), Range(0, 20, ErrorMessage = "The Experience range should be 0 to 20"), DefaultValue(10.0)]
        public decimal TotalExperience { get; set; } = 22;

        [Required, Display(Name = "Select a Country"), DataType("DropdownList"), LinkedAttribute(typeof(int))]
        public string Country { get; set; }

        [Required, DataType(DataType.MultilineText), Display(Name = "Address"), DefaultValue("piazza 24 maggio"), BlazorDynamicFormGenerator.ReadOnly]
        public string Address { get; set; }
    }
```
Generate the model description:
```csharp
var definition = ModuleNodePropertyDefinitionExtensions.GetDefinition<Test>();
```
Get the result direclty as JSON:
```csharp
 void OnValidResult(string data){..}
```

Result:
```json
{"FirstName":"Lorenzo","LastName":null,"Email":"l@hoy.com","PhoneNumber":"331","DOB":"2023-06-22T00:00:00+02:00","TotalExperience":10.0,"Country":"id","Address":"piazza 24 maggio"}
```

Showcase with Boostrap and Syncfusion
|  |  |
|:---:|:---:|
| ![image](https://github.com/TheSmallPixel/BlazorDynamicForm/assets/25280244/8cfc9458-681b-49ce-a2e6-0cebffe7364e) | ![image](https://github.com/TheSmallPixel/BlazorDynamicForm/assets/25280244/f802568d-ebde-4e03-8bd2-30e5cc34804b) |

## Data Providers

The `AddDataProvider` method is used to set up a source of data for certain elements in the form, particularly for elements like dropdown lists or combo boxes that require a list of options to present to the user.

This method accepts a delegate that can retrieve or generate the data required for a given attribute and name. The method should return the data in a format that can be used to populate the dropdown list or other similar components.

Here's how it is used in the given code example:

```csharp
builder.Services.AddBlazorDynamicForm()
    .AddDataProvider((attribute, name) =>
    {
        var data = new List<FormVar>();
        data.Add(new FormVar() { Id = "id", Name = "Cipolle" });
        return data;
    })
    .SyncfusionForm();
```

In this example, `AddDataProvider` is being used to provide a list of `FormVar` objects whenever needed. In a more realistic scenario, the delegate might retrieve the data from a database or an API depending on the attribute and name provided.

This makes `AddDataProvider` a powerful tool for dynamic form generation, as it allows you to tailor the data shown in each field based on the requirements of that field.

## Create a Custom Integration
SyncfusionIntegration is a static class that serves as an example of how to extend the Dynamic Form Configuration to integrate custom data types and components.

The SyncfusionForm method of this class modifies the form's configuration, adding a series of Syncfusion Blazor components with their respective data types. The attributes of these components are set through various builder functions.



```csharp
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
```

## Add Method

The `Add` method is used to map a data annotation attribute to a Blazor component. This method accepts the type of the component, the DataType for which the component should be used, and an optional configuration delegate.

Here's an example of how it is used:

```csharp
config.Add<SfTextBox, string>(DataType.Text, (builder, sequence, attribute) =>
{
    builder.AddAttribute(sequence++, "Readonly", attribute.ReadOnly);
});
```

## AddCustom Method

The `AddCustom` method allows for more advanced configurations. It is used when you want to map a custom data annotation to a specific Blazor component. This method also accepts the type of the component, the custom data annotation name, and a configuration delegate.

Here's an example:

```csharp
config.AddCustom<SfDropDownList<string, FormVar>, string>("DropdownList", (builder, sequence, attribute) =>
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
});
```

In the above example, the `AddCustom` method maps a custom data annotation named "DropdownList" to a `SfDropDownList` component. The configuration delegate is used to configure the `DataSource` and `ChildContent` attributes of the `SfDropDownList` component.

These two methods are very powerful as they give you the ability to customize the way your forms work with different data types and annotations, leveraging the variety of components provided by Syncfusion Blazor.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. Please make sure to update tests as appropriate.

## License

This project is licensed under the GPL-3.0 license. See the [LICENSE](LICENSE) file for details.

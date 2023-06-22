# BlazorDynamicFormSyncfusion

BlazorDynamicFormSyncfusion is a dynamic form generator built with C# for Blazor applications, with an integration of the Syncfusion Blazor UI library. It uses data annotation attributes to automatically build forms with validation included.

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
NuGet\Install-Package BlazorDynamicFormDataAnnotation -Version 1.0.4
NuGet\Install-Package BlazorDynamicFormGenerator -Version 1.0.4
NuGet\Install-Package BlazorDynamicFormSyncfusion -Version 1.0.4
```
## Packages
- BlazorDynamicFormDataAnnotation: It takes care of generating the forms for blazor.
- BlazorDynamicFormGenerator: Generate the class description template, so the frontend doesn't have to have the class or it can be dynamic.
- BlazorDynamicFormSyncfusion: Is an extension to allow you to integrate components dedicated to data type or attribute.

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
## Showcase with Boostrap and Syncfusion
|  |  |
|:---:|:---:|
| ![image](https://github.com/TheSmallPixel/BlazorDynamicForm/assets/25280244/8cfc9458-681b-49ce-a2e6-0cebffe7364e) | ![image](https://github.com/TheSmallPixel/BlazorDynamicForm/assets/25280244/f802568d-ebde-4e03-8bd2-30e5cc34804b) |




## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. Please make sure to update tests as appropriate.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

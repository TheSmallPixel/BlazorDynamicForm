# BlazorDynamicFormSyncfusion

BlazorDynamicFormSyncfusion is a dynamic form generator built with C# for Blazor applications, with an integration of the Syncfusion Blazor UI library. It uses data annotation attributes to automatically build forms with validation included.

## Features

- Built-in data annotations support for forms.
- Syncfusion Blazor components integration.
- Customization options for form fields.
- Automatic form generation and handling.

## Installation

First, make sure you have the .NET SDK installed. Then, clone the repository and open it in your preferred IDE.

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

```csharp
@using BlazorDynamicFormDataAnnotation
```

Here is an example of a dynamic form in a Razor component:

```razor
<DynamicForm DataDefinition="@definition" DataObject="@data" OnValidSubmit="@PrintResult">
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

## Running the Project

To run the project, use the `dotnet run` command in your terminal from the root directory of the project.

```bash
dotnet run
```

Open your web browser and navigate to `https://localhost:5001` (or `http://localhost:5000` for non-secure HTTP) to see the application running.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. Please make sure to update tests as appropriate.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

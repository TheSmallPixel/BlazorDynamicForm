# BlazorDynamicForm

A lightweight, flexible form generator for Blazor applications that creates dynamic forms from annotated C# classes.

## Features

- Creates forms automatically from C# classes
- Built-in validation through data annotations
- Customizable form rendering and layout
- Type-safe form handling

## Installation

```bash
dotnet add package BlazorDynamicForm --version 2.0.4
```

## Quick Start

1. **Add Service in Program.cs**

```csharp
builder.Services.AddBlazorDynamicForm();
```

2. **Create a Model with Annotations**

```csharp
public class ContactForm
{
    [Required, Display(Name = "Name")]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [Phone, Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    
    [TextArea]
    public string Message { get; set; }
}
```

3. **Use the DynamicForm Component**

```razor
@using BlazorDynamicForm.Components
@using TypeAnnotationParser

<DynamicForm Scheme="@_formScheme" Data="_formData" OnValidSubmit="@HandleSubmit">
    <SubmitTemplate>
        <button type="submit" class="btn btn-primary">Submit</button>
    </SubmitTemplate>
</DynamicForm>

@code {
    private SchemeModel _formScheme;
    private IDictionary<string, object> _formData = new Dictionary<string, object>();
    
    protected override void OnInitialized()
    {
        var parser = new TypeAnnotationParser();
        _formScheme = parser.Parse<ContactForm>();
    }
    
    private void HandleSubmit(IDictionary<string, object> data)
    {
        // Handle form data
    }
}
```
4. **Example**
   ![Screenshot 2025-05-06 170325](https://github.com/user-attachments/assets/a0df0795-c34c-491b-8bfd-e7fe91f6453e)


## Custom Attributes

BlazorDynamicForm provides additional attributes for enhanced form customization:

- `[TextArea]` - Creates a multi-line text input
- `[Placeholder("Enter text...")]` - Adds placeholder text
- `[Grid(6)]` - Controls the layout grid width
- `[Name("Custom Field Name")]` - Sets a custom field name
- `[MultipleSelect("Option1", "Option2", "Option3")]` - Creates a dropdown with options

## License

[GPL-3.0](https://www.gnu.org/licenses/gpl-3.0.html)

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

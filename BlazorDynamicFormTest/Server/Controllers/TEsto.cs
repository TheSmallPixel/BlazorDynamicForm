using BlazorDynamicForm.Validation;

namespace BlazorDynamicFormTest.Server.Controllers;

public class TEsto
{
    [DisplayNameForm("Ciao")]
    public string Name { get; set; }
    [DisplayNameForm("Ciao")]
    public string Surname { get; set; }
    [DisplayNameForm("Ciao")]
    public string Baba { get; set; }                  
    public Lol test2 { get; set; }

    [DataType(DataTypeAttribute.FormDatatype.Dictionary), FormDictionary(nameof(Lol.Name))]
    public List<Lol> test3 { get; set; }
}
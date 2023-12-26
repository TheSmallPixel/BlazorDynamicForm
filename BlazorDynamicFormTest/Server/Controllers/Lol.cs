using BlazorDynamicForm.Validation;

namespace BlazorDynamicFormTest.Server.Controllers;

public class Lol
{
    [PlaceholderForm("Ciao"), DefaultValue("Ciao")]
    public string Name { get; set; }
    [DisplayNameForm("Ciao")]
    public string Surname { get; set; }
    [DisplayNameForm("Ciao")]
    [MultipleSelectForm(new []{"AAA","BBB"})]
    public string Baba { get; set; }                
    public List<TEsto> ThisIsNiceDay2 { get; set; }   
    public List<Lol> ThisIsNiceDay4 { get; set; }
    public List<TEsto> test2 { get; set; }
}
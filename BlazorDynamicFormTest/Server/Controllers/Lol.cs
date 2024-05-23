 using BlazorDynamicForm.Attributes.Display;


namespace BlazorDynamicFormTest.Server.Controllers;

public class Lol
{
    [PlaceholderForm("Ciao"), DefaultValueForm("Ciao")]
    public string Name { get; set; }
    [DisplayNameForm("Ciao")]
    public string Surname { get; set; }
    [DisplayNameForm("Ciao")]
    [MultipleSelectForm(new []{"AAA","BBB"})]
    public string Baba { get; set; }                
    //public List<TEsto> ThisIsNiceDay2 { get; set; }   
    //public List<Lol> ThisIsNiceDay4 { get; set; }
    //public List<TEsto> test2 { get; set; }
    //public Dictionary<int,TEsto> test3 { get; set; }
    //public Dictionary<TEsto, TEsto> test4 { get; set; }
    //public Dictionary<TEsto, string> test5 { get; set; }
    //public int[] test6 { get; set; }
    //public TEsto[] test7 { get; set; }
    public Lol teeeest { get; set; }
}
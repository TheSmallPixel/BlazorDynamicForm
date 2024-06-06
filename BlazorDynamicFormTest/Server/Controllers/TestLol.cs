using BlazorDynamicForm.Attributes.Display;

namespace BlazorDynamicFormTest.Server.Controllers;

public class TestLol
{
    public TEsto test2 { get; set; }  
    public Dictionary<string, SwitchOption> Options { get; set; }
    public Dictionary<string, Lol> testDic { get; set; } 
    public List<List<List<int>>> test3 { get; set; }
    public string Surname { get; set; }

    public int Age { get; set; }
    public Lol lol { get; set; }
    public List<Lol> looool { get; set; }

   // [File]
    public FileData FileData { get; set; }

    [CodeEditor(Language = "javascript")]
    public string JavascriptShit { get; set; }

    [MultipleSelect("Yes", "no", "boh")]
    public string MultipleSelectMeee { get; set; }
}

public class SwitchOption
{
    [MultipleSelect("==", "like", "None")]
    public string MatchType { get; set; }

    public string Var { get; set; }
}
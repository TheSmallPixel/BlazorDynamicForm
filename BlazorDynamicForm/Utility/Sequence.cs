namespace BlazorDynamicForm.Utility;

public class Sequence
{
    public int Value { get; set; }

    public static implicit operator int(Sequence i) => i.Value;

    public static Sequence operator ++(Sequence left)
    {
        left.Value++;
        return left;
    }
    public override string ToString()
    {
        return Value.ToString();
    }
}
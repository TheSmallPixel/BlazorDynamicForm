namespace TypeAnnotationParser;

public record ParserConfiguration
{
    public List<Annotation> Attributes { get; set; } = new();
}
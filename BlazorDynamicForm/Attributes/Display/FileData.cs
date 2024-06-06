﻿namespace BlazorDynamicForm.Attributes.Display;

public record FileData
{
    public string Name { get; set; }
    public string ContentType { get; set; }
    public string Data { get; set; }
}
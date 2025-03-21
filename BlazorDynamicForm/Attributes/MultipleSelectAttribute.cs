﻿using BlazorDynamicForm.Core;
using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public class MultipleSelectAttribute : DynamicRendererComponent
{
    public string[] Options { get; set; }
    public MultipleSelectAttribute() { }

    public MultipleSelectAttribute(params string[] options)
    {
        Options = options;
    }
}
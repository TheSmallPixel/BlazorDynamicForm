﻿using TypeAnnotationParser;

namespace BlazorDynamicForm.Attributes;

public class NameAttribute : AttributeScheme
{
	public NameAttribute() { }

	public NameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
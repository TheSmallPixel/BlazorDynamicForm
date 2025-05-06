using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeAnnotationParser;
using static BlazorDynamicForm.Attributes.LabelAttribute;

namespace BlazorDynamicForm.Attributes
{
	public class GridAttribute : AttributeScheme
	{
		public GridAttribute() { }

		public GridAttribute(int size)
		{
			Size = size;
		}

		public int Size { get; set; } = 12;
	}
	public class LabelAttribute : AttributeScheme
	{
		public enum LabelPosition {None, Inline, Top}
		public LabelAttribute() { }

		public LabelAttribute(string label, LabelPosition position = LabelPosition.Top)
		{
			Label = label;
			Position = position;
		}

		public string Label { get; set; }
		public LabelPosition Position { get; set; }

		public static LabelAttribute Instance => new LabelAttribute(string.Empty, LabelPosition.None);
	}

	public class BoxAttribute : AttributeScheme
	{
		public enum BoxVisibility { None, Visible }
		public BoxAttribute() { }

		public BoxAttribute(BoxVisibility visibility = BoxVisibility.Visible)
		{
			Visibility = visibility;
		}

		public BoxVisibility Visibility { get; set; }

		public static BoxAttribute Instance => new BoxAttribute();
	}
}

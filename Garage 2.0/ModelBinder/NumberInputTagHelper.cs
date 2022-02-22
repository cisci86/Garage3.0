using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Mvc.TagHelpers
{
	/// <summary>
	/// Adds additional behavior to the modelbinding for numeric properties.
	/// </summary>
	[HtmlTargetElement("input", Attributes = "asp-for")]
	public class NumberInputTagHelper : InputTagHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NumberInputTagHelper"/> class.
		/// </summary>
		/// <param name="generator">The HTML generator.</param>
		public NumberInputTagHelper(IHtmlGenerator generator)
			: base(generator)
		{ }

		/// <inheritdoc />
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			base.Process(context, output);

			var types = new[] {
				typeof(byte), typeof(sbyte),
				typeof(ushort), typeof(short),
				typeof(uint), typeof(int),
				typeof(ulong), typeof(long),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};

			var typeAttributes = output.Attributes
				.Where(a => a.Name == "type")
				.ToList();
			string typeAttributeValue = typeAttributes.First().Value as string;

			Type modelType = For.ModelExplorer.ModelType;
			Type nullableType = Nullable.GetUnderlyingType(modelType);

			// the type itself or its nullable wrapper matching and
			// the type attribute is number or there is only one type attribute
			// IMPORTANT TO KNOW: if the type attribute is set in the view, there are two attributes with same value.
			if ((types.Contains(modelType) || types.Contains(nullableType)) && (typeAttributeValue == "number" || typeAttributes.Count == 1))
			{
				var culture = CultureInfo.InvariantCulture;
				string min = "";
				string max = "";
				string step = "";
				string value = "";

				if (modelType == typeof(byte) || nullableType == typeof(byte))
				{
					min = byte.MinValue.ToString(culture);
					max = byte.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						byte val = (byte)For.Model;
						value = value.ToString(culture);
					}
				}
				else if (modelType == typeof(sbyte) || nullableType == typeof(sbyte))
				{
					min = sbyte.MinValue.ToString(culture);
					max = sbyte.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						sbyte val = (sbyte)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(ushort) || nullableType == typeof(ushort))
				{
					min = ushort.MinValue.ToString(culture);
					max = ushort.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						ushort val = (ushort)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(short) || nullableType == typeof(short))
				{
					min = short.MinValue.ToString(culture);
					max = short.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						short val = (short)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(uint) || nullableType == typeof(uint))
				{
					min = uint.MinValue.ToString(culture);
					max = uint.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						uint val = (uint)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(int) || nullableType == typeof(int))
				{
					min = int.MinValue.ToString(culture);
					max = int.MaxValue.ToString(culture);

					if (For.Model != null)
					{
						int val = (int)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(ulong) || nullableType == typeof(ulong))
				{
					min = ulong.MinValue.ToString(culture);

					if (For.Model != null)
					{
						ulong val = (ulong)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(long) || nullableType == typeof(long))
				{
					if (For.Model != null)
					{
						long val = (long)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(float) || nullableType == typeof(float))
				{
					step = "any";

					if (For.Model != null)
					{
						float val = (float)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(double) || nullableType == typeof(double))
				{
					step = "any";

					if (For.Model != null)
					{
						double val = (double)For.Model;
						value = val.ToString(culture);
					}
				}
				else if (modelType == typeof(decimal) || nullableType == typeof(decimal))
				{
					step = "any";

					if (For.Model != null)
					{
						decimal val = (decimal)For.Model;
						value = val.ToString(culture);
					}
				}

				output.Attributes.SetAttribute(new TagHelperAttribute("type", "number"));
				output.Attributes.SetAttribute(new TagHelperAttribute("value", value));

				if (!string.IsNullOrWhiteSpace(min) && !output.Attributes.ContainsName("min"))
					output.Attributes.SetAttribute(new TagHelperAttribute("min", min));

				if (!string.IsNullOrWhiteSpace(max) && !output.Attributes.ContainsName("max"))
					output.Attributes.SetAttribute(new TagHelperAttribute("max", max));

				if (!string.IsNullOrWhiteSpace(step) && !output.Attributes.ContainsName("step"))
					output.Attributes.SetAttribute(new TagHelperAttribute("step", step));
			}
		}
	}
}
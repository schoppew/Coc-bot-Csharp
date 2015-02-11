namespace CoC.Bot.UI.Converters
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Windows.Data;

	public class EnumToDescriptionConverter : IValueConverter
	{
		private string GetEnumDescription(Enum enumObj)
		{
			FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

			object[] attribArray = fieldInfo.GetCustomAttributes(false);

			if (attribArray.Length == 0)
			{
				return enumObj.ToString();
			}
			else
			{
				DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
				return attrib.Description;
			}
		}

		/// <summary>
		/// Converts the value to Point.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Enum myEnum = (Enum)value;
			string description = GetEnumDescription(myEnum);
			return description;
		}

		/// <summary>
		/// Converts back the value to string.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Empty;
		}
	}
}
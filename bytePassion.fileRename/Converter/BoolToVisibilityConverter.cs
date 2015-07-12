using System;
using System.Globalization;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.GenericValueConverter;


namespace bytePassion.FileRename.Converter
{
	public class BoolToVisibilityConverter : GenericValueConverter<bool, Visibility>
	{
		protected override Visibility Convert (bool value, CultureInfo culture)
		{
			return value ? Visibility.Visible : Visibility.Collapsed;
		}

		protected override bool ConvertBack(Visibility value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

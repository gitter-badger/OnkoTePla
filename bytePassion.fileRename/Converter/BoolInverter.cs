using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.GenericValueConverter;


namespace bytePassion.FileRename.Converter
{
	public class BoolInverter : GenericValueConverter<bool, bool>
	{
		protected override bool Convert(bool value, CultureInfo culture)
		{
			return !value;
		}

		protected override bool ConvertBack(bool value, CultureInfo culture)
		{
			return !value;
		}
	}
}

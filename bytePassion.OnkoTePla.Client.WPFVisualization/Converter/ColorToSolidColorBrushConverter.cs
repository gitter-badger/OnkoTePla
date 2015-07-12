using System;
using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.GenericValueConverter;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class ColorToSolidColorBrushConverter : GenericValueConverter<Color, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(Color value, CultureInfo culture)
		{
			return new SolidColorBrush(value);
		}

		protected override Color ConvertBack(SolidColorBrush value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

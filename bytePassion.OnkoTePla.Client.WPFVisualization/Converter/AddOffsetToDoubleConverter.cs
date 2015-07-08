using System;
using System.Globalization;
using bytePassion.Lib.GenericValueConverter;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class AddOffsetToDoubleConverter : GenericParameterizedValueConverter<double, double, string>
	{
		protected override double Convert(double value, string parameter, CultureInfo culture)
		{
			return value + Double.Parse(parameter);
		}

		protected override double ConvertBack(double value, string parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

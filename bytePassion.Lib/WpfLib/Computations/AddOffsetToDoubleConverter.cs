using bytePassion.Lib.WpfLib.ConverterBase;
using System;
using System.Globalization;


namespace bytePassion.Lib.WpfLib.Computations
{
	public class AddOffsetToDoubleConverter : GenericParameterizedValueConverter<double, double, string>
	{
		protected override double Convert(double value, string parameter, CultureInfo culture)
		{
			return value + Double.Parse(parameter);
		}		
	}
}

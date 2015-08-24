using System;
using System.Globalization;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.Lib.WpfUtils.Computations
{
	public class AddOffsetToDoubleConverter : GenericParameterizedValueConverter<double, double, string>
	{
		protected override double Convert(double value, string parameter, CultureInfo culture)
		{
			return value + Double.Parse(parameter);
		}		
	}
}

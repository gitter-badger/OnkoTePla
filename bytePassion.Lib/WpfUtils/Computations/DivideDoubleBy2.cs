using System.Globalization;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.Lib.WpfUtils.Computations
{
    public class DivideDoubleBy2 : GenericValueConverter<double, double>
    {
	    protected override double Convert(double value, CultureInfo culture)
	    {
		    return value / 2.0;
	    }

	    protected override double ConvertBack(double value, CultureInfo culture)
	    {
		    return value * 2.0;
	    }
    }
}

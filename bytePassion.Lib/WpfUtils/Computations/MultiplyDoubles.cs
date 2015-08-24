using System.Globalization;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.Lib.WpfUtils.Computations
{
	public class MultiplyDoubles : GenericTwoToOneValueConverter<double, double, double>
    {
	    protected override double Convert(double value1, double value2, CultureInfo culture)
	    {
		    return value1 * value2;
	    }	   
    }
}

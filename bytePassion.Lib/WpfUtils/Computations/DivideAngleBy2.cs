using bytePassion.Lib.MathLib;
using bytePassion.Lib.WpfUtils.ConverterBase;
using System.Globalization;


namespace bytePassion.Lib.WpfUtils.Computations
{
    public class DivideAngleBy2 : GenericParameterizedValueConverter<Angle, double, bool>
    {	   
	    protected override double Convert(Angle value, bool invert, CultureInfo culture)
	    {
			return (invert ? -1 : 1) * value.Value / 2.0;
	    }	   
    }
}

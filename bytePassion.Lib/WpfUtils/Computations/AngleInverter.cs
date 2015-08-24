using System.Globalization;
using bytePassion.Lib.Math;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.Lib.WpfUtils.Computations
{
    public class AngleInverter : GenericValueConverter<Angle, double>
    {
	    protected override double Convert(Angle value, CultureInfo culture)
	    {
		    return 360.0 - value.PosValue.Value;
	    }	    
    }
}

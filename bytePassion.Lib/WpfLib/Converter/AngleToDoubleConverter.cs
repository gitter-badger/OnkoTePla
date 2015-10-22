using bytePassion.Lib.GeometryLib.Base;
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;


namespace bytePassion.Lib.WpfLib.Converter
{
    public class AngleToDoubleConverter : GenericValueConverter<Angle, double>
    {
	    protected override double Convert(Angle angle, CultureInfo culture)
	    {
		    return angle.Value;
	    }

	    protected override Angle ConvertBack(double value, CultureInfo culture)
	    {
		    return new Angle(value);
	    }
    }
}

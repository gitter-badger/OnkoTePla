using bytePassion.Lib.MathLib;
using bytePassion.Lib.WpfUtils.ConverterBase;
using System.Globalization;


namespace bytePassion.Lib.WpfUtils.Converter
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

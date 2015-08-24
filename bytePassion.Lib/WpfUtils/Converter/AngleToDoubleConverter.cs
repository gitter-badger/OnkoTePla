using System.Globalization;
using bytePassion.Lib.Math;
using bytePassion.Lib.WpfUtils.ConverterBase;


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

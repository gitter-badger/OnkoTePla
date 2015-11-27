using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;


namespace bytePassion.Lib.WpfLib.Converter
{
    public class XCoordToDoubleConverter : GenericValueConverter<XCoord, double>
    {
        protected override double Convert(XCoord value, CultureInfo culture)
        {
            return value.Value;
        }

        protected override XCoord ConvertBack(double value, CultureInfo culture)
        {
            return new XCoord(value);
        }
    }
}

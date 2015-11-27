using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;


namespace bytePassion.Lib.WpfLib.Converter
{

    public class YCoordToDoubleConverter : GenericValueConverter<YCoord, double>
    {
        protected override double Convert(YCoord value, CultureInfo culture)
        {
            return value.Value;
        }

        protected override YCoord ConvertBack(double value, CultureInfo culture)
        {
            return new YCoord(value);
        }
    }
}
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;
using System.Windows;


namespace bytePassion.Lib.WpfLib.Converter
{
    public class InvertedBoolToCollapsedVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
			return value ? Visibility.Collapsed : Visibility.Visible;
	    }	    
    }
}

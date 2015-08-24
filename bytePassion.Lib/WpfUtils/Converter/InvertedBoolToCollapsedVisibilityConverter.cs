using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.Lib.WpfUtils.Converter
{
    public class InvertedBoolToCollapsedVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
			return value ? Visibility.Collapsed : Visibility.Visible;
	    }	    
    }
}

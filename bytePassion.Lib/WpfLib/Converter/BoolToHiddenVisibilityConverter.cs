using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;
using System.Windows;


namespace bytePassion.Lib.WpfLib.Converter
{
    public class BoolToHiddenVisibilityConverter : GenericValueConverter<bool, Visibility>
    {
	    protected override Visibility Convert(bool value, CultureInfo culture)
	    {
		    return value ? Visibility.Visible : Visibility.Hidden;
	    }	   
    }
}

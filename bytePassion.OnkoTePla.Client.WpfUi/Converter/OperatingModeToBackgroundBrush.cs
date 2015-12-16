using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using System.Globalization;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
    public class OperatingModeToBackgroundBrush : GenericValueConverter<OperatingMode, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(OperatingMode value, CultureInfo culture)
		{
			return value == OperatingMode.Edit ? new SolidColorBrush(Colors.Firebrick) : new SolidColorBrush(Colors.Orange);
		}		
	}
}

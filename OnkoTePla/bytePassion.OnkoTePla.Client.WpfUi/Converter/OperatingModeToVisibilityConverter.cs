using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
	internal class OperatingModeToVisibilityConverter : GenericValueConverter<OperatingMode, Visibility>
	{
		protected override Visibility Convert(OperatingMode value, CultureInfo culture)
		{
			return value == OperatingMode.Edit ? Visibility.Visible : Visibility.Collapsed;
		}		
	}
}

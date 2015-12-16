using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using System.Globalization;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class OperatingModeToVisibilityConverter : GenericValueConverter<OperatingMode, Visibility>
	{
		protected override Visibility Convert(OperatingMode value, CultureInfo culture)
		{
			return value == OperatingMode.Edit ? Visibility.Visible : Visibility.Collapsed;
		}		
	}
}

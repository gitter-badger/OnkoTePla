using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfUtils.ConverterBase;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;


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

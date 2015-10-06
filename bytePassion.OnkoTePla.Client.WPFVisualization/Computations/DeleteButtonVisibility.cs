using System.Globalization;
using System.Windows;
using bytePassion.Lib.WpfUtils.ConverterBase;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Computations
{
	public class DeleteButtonVisibility : GenericTwoToOneValueConverter<OperatingMode, bool, Visibility>
	{
		protected override Visibility Convert(OperatingMode operatingMode, bool showDisabledOverlay, CultureInfo culture)
		{
			if (showDisabledOverlay)
				return Visibility.Collapsed;

			return operatingMode == OperatingMode.Edit ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}

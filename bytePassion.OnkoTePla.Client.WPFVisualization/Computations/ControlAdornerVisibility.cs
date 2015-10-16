using System.Globalization;
using bytePassion.Lib.WpfUtils.ConverterBase;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Computations
{
	public class ControlAdornerVisibility : GenericTwoToOneValueConverter<OperatingMode, bool, bool>
	{
		protected override bool Convert(OperatingMode operatingMode, bool showDisabledOverlay, CultureInfo culture)
		{
			if (showDisabledOverlay)
				return false;

			return operatingMode == OperatingMode.Edit;
		}
	}
}

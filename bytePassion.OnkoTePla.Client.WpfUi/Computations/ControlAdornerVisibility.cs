using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using System.Globalization;


namespace bytePassion.OnkoTePla.Client.WpfUi.Computations
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

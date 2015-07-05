using System;
using System.Globalization;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class AppointmentGridModeToVisibilityConverter : GenericValueConverter<AppointmentGridViewMode, Visibility>
	{
		protected override Visibility Convert(AppointmentGridViewMode value, CultureInfo culture)
		{
			return value == AppointmentGridViewMode.Edit ? Visibility.Visible : Visibility.Collapsed;
		}

		protected override AppointmentGridViewMode ConvertBack(Visibility value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

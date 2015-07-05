using System;
using System.Globalization;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class OperatingModeToVisibilityConverter : GenericValueConverter<OperatingMode, Visibility>
	{
		protected override Visibility Convert(OperatingMode value, CultureInfo culture)
		{
			return value == OperatingMode.Edit ? Visibility.Visible : Visibility.Collapsed;
		}

		protected override OperatingMode ConvertBack(Visibility value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

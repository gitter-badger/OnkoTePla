using System;
using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class OperatingModeToBackgroundBrush : GenericValueConverter<OperatingMode, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(OperatingMode value, CultureInfo culture)
		{
			return value == OperatingMode.Edit ? new SolidColorBrush(Colors.Firebrick) : new SolidColorBrush(Colors.Orange);
		}

		protected override OperatingMode ConvertBack(SolidColorBrush value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}		
	}
}

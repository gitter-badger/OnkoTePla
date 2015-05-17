using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class AliveDeadToImageConverter : GenericValueConverter<bool, BitmapImage>
	{
		protected override BitmapImage Convert(bool alive, CultureInfo culture)
		{
			if (alive)
				return new BitmapImage(new Uri(@"pack://application:,,,/bytePassion.OnkoTePla.Client.Resources;Component/Icons/AliveDead/alive.png"));
			else
				return new BitmapImage(new Uri(@"pack://application:,,,/bytePassion.OnkoTePla.Client.Resources;Component/Icons/AliveDead/dead.png"));
		}

		protected override bool ConvertBack(BitmapImage value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}				
	}
}

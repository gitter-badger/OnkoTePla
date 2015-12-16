using bytePassion.Lib.WpfLib.ConverterBase;
using System;
using System.Globalization;
using System.Windows.Media.Imaging;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
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
	}
}

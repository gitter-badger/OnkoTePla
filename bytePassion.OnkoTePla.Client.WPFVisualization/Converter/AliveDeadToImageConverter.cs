using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class AliveDeadToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool)) throw new ArgumentException();

			bool alive = (bool)value;

			if (alive)
				return new BitmapImage(new Uri(@"pack://application:,,,/bytePassion.OnkoTePla.Client.Resources;Component/Icons/AliveDead/alive.png"));			
			else
				return new BitmapImage(new Uri(@"pack://application:,,,/bytePassion.OnkoTePla.Client.Resources;Component/Icons/AliveDead/dead.png"));						
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

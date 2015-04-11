using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace xIT.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class AliveDeadToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool)) throw new ArgumentException();

			bool alive = (bool)value;

			if (alive)
				return new BitmapImage(new Uri(@"\Icons\AliveDead\alive.png", UriKind.Relative));			
			else
				return new BitmapImage(new Uri(@"\Icons\AliveDead\dead.png", UriKind.Relative));						
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

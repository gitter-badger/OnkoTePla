using System;
using System.Globalization;
using System.Windows.Data;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class BirthdayToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is DateTime)) throw new ArgumentException();

			var valueAsDateTime = (DateTime) value;

			// TODO: CultureInfo nicht hard coden!
			return valueAsDateTime.ToString("d", new CultureInfo("de-DE"));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

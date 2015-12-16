using System;
using System.Globalization;
using System.Windows.Data;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
    class TimeFromDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = value as DateTime?;
            return date?.ToString("H:mm") ?? "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

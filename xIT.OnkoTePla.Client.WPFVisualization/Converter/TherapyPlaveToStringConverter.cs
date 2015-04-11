using System;
using System.Globalization;
using System.Windows.Data;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class TherapyPlaceToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TherapyPlace)) throw new ArgumentException();

			var valueAsTherapyPlace = (TherapyPlace) value;
			return valueAsTherapyPlace.TherapyPlaceID + " (" + valueAsTherapyPlace.TherapyPlaceType.TypeName + ")";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	} 
}

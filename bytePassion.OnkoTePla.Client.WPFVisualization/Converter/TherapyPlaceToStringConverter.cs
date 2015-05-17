using System;
using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class TherapyPlaceToStringConverter : GenericValueConverter<TherapyPlace, string>
	{
		protected override string Convert(TherapyPlace therapyPlace, CultureInfo culture)
		{
			return therapyPlace.TherapyPlaceID + " (" + therapyPlace.TherapyPlaceType.TypeName + ")";
		}

		protected override TherapyPlace ConvertBack(string value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}		
	} 
}

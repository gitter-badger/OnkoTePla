using System.Globalization;
using bytePassion.Lib.WpfUtils.ConverterBase;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class TherapyPlaceToStringConverter : GenericValueConverter<TherapyPlace, string>
	{
		protected override string Convert(TherapyPlace therapyPlace, CultureInfo culture)
		{
			return therapyPlace.Id + " (" + therapyPlace.Type.Name + ")";
		}		
	} 
}

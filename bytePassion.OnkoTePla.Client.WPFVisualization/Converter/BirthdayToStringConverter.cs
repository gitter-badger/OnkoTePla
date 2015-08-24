using System.Globalization;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class BirthdayToStringConverter : GenericValueConverter<Date, string>
	{
		protected override string Convert(Date date, CultureInfo culture)
		{
			// TODO: CultureInfo nicht hard coden!
		    return date.GetDisplayString(CultureInfo.CurrentCulture);
		}		
	}
}

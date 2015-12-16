using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
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

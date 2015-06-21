using System;
using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class BirthdayToStringConverter : GenericValueConverter<Date, string>
	{
		protected override string Convert(Date date, CultureInfo culture)
		{
			// TODO: CultureInfo nicht hard coden!
		    return date.GetDisplayString(CultureInfo.CurrentCulture);
		}

		protected override Date ConvertBack(string value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	internal class BirthdayToStringConverter : GenericValueConverter<DateTime, string>
	{
		protected override string Convert(DateTime date, CultureInfo culture)
		{
			// TODO: CultureInfo nicht hard coden!
			return date.ToString("d", new CultureInfo("de-DE"));
		}

		protected override DateTime ConvertBack(string value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

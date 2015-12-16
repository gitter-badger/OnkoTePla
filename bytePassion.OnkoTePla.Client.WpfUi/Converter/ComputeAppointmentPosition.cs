using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfLib.ConverterBase;
using System.Globalization;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
    public class ComputeAppointmentPosition : GenericThreeToOneValueConverter<Time, Time, double, double>
	{
		protected override double Convert(Time appointmentStartTime, Time slotStartTime, double lengthOfOneHour, CultureInfo culture)
		{
			var durationFromDayBeginToAppointmentStart = new Duration(appointmentStartTime, slotStartTime);
			return lengthOfOneHour * durationFromDayBeginToAppointmentStart.Seconds * 3600;
		}		
	}
}

using System.Globalization;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.ConverterBase;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
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

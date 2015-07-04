using System;
using System.Globalization;
using bytePassion.Lib.TimeLib;
using funkwerk.phx.smp.map;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Converter
{
	public class ComputeAppointmentPosition : GenericThreeToOneValueConverter<Time, Time, double, double>
	{
		protected override double Convert(Time appointmentStartTime, Time slotStartTime, double lengthOfOneHour, CultureInfo culture)
		{
			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointmentStartTime, slotStartTime);
			return lengthOfOneHour * durationFromDayBeginToAppointmentStart.Seconds * 3600;
		}

		protected override object[] ConvertBack(double value, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

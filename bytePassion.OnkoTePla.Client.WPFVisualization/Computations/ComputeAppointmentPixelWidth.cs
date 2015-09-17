﻿using System.Globalization;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.ConverterBase;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Computations
{
	public class ComputeAppointmentPixelWidth : GenericFiveToOneValueConverter<Time, Time, double, Time, Time, double>
	{
		protected override double Convert(Time startingTime, Time endingTime, double gridWidth, Time timeSlotStart, Time timeSlotEnd, CultureInfo culture)
		{
			var lengthOfOneHour = gridWidth / (Time.GetDurationBetween(timeSlotEnd, timeSlotStart).Seconds / 3600.0);															
			var durationOfAppointment = Time.GetDurationBetween(startingTime, endingTime);

			return lengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
		}
	}
}
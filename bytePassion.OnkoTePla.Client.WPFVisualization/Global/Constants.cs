﻿using System.Windows;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Global
{
	public static class Constants
	{
		// Global ViewModel-Vairables

		public const string AppointmentGridSizeVariable              = "AppointmentGridSize";
		public const string AppointmentGridSelectedDateVariable      = "AppointmentGridSelectedDate";
		public const string AppointmentGridDisplayedPracticeVariable = "AppointmentGridDisplayedPractice";
		public const string AppointmentGridRoomFilterVariable        = "AppointmentGridSelectedRoom";
		public const string SideBarStateVariable                     = "SideBarState";
		public const string SelectedAppointmentVariable				 = "SelectedAppointment";


		// ViewModel-Collections

		public const string TherapyPlaceRowViewModelCollection = "TherpyPlaceRowCollection";
		public const string AppointmentGridViewModelCollection = "AppointmentGridCollection";
		public const string TimeGridViewModelCollection        = "TimeGridCollection";
		public const string AppointmentViewModelCollection     = "AppointmentCollection";

		
		// View-Constants

		public static readonly GridLength AppointmentGridLeftColumsSize = new GridLength(150.0, GridUnitType.Pixel);
		public static readonly GridLength AppointmentGridTopRowSizw     = new GridLength( 50.0, GridUnitType.Pixel);

		
		// Thresholds to add or remove TimeGridLines

		public const double ThresholdGridWidthQuarterHoursToHalfHours = 1400;
		public const double ThresholdGridWidthHalfHoursToHours        = 1000;
		public const double ThresholdGridWidthHoursToTwoHours         =  600;

	}
}
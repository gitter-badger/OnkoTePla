using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Messages
{
	public class AddAppointmentToTherapyPlaceRow : ViewModelMessageBase
	{
		public AddAppointmentToTherapyPlaceRow(AppointmentViewModel appointmentViewModelToAdd, 
											   Date destinationDay, Guid destinationRow)
		{
			AppointmentViewModelToAdd = appointmentViewModelToAdd;
			DestinationDay = destinationDay;
			DestinationRow = destinationRow;
		}

		public AppointmentViewModel AppointmentViewModelToAdd { get; }
		public Date DestinationDay { get; }
		public Guid DestinationRow { get; }
	}
}

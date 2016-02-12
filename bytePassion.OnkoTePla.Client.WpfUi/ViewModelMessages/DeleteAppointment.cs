using System;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages
{
	public class DeleteAppointment : ViewModelMessage
    {
	    
	    public DeleteAppointment(Guid appointmentId, Guid patientId, ActionTag actionTag)
	    {
		    AppointmentId = appointmentId;
		    PatientId = patientId;
		    ActionTag = actionTag;
	    }

		public Guid      AppointmentId { get; }
		public Guid      PatientId     { get; }
		public ActionTag ActionTag     { get; }
	}
}

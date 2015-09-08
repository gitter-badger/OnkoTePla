using System;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class DeleteAppointment : ViewModelMessage
    {
	    
	    public DeleteAppointment(Guid appointmentId, Guid patientId)
	    {
		    AppointmentId = appointmentId;
		    PatientId = patientId;
	    }

		public Guid AppointmentId { get; }
		public Guid PatientId     { get; }
	}
}

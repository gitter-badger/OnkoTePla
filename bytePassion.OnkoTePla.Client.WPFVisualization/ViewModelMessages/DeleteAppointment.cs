using System;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class DeleteAppointment
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

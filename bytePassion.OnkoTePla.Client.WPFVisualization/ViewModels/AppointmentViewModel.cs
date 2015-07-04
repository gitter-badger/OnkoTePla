using System.ComponentModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentViewModel : IAppointmentViewModel
	{
		private Appointment appointment;

		public AppointmentViewModel(Appointment appointment)
		{
			this.appointment = appointment;
		}

		public string PatientDisplayName
		{
			get { return appointment.Patient.Name; }
		}

		public double FrameworkElementWidth { get; private set; }

		public Time StartTime { get; set; }
		public Time EndTime   { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

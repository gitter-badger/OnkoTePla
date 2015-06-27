using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}

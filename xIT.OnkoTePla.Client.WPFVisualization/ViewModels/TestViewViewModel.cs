using System.Collections.Generic;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class TestViewViewModel : ITestViewViewModel
	{
		private readonly IReadOnlyList<TherapyPlace> therapyPlaces;
		private readonly IReadOnlyList<Patient> patients;
		private readonly IReadOnlyList<Appointment> appointments;

		public TestViewViewModel (IReadOnlyList<TherapyPlace> therapyPlaces, IReadOnlyList<Patient> patients,
								 IReadOnlyList<Appointment> appointments)
		{
			this.therapyPlaces = therapyPlaces;
			this.patients = patients;
			this.appointments = appointments;
		}

		public IReadOnlyList<TherapyPlace> TherapyPlaces
		{
			get { return therapyPlaces; }
		}

		public IReadOnlyList<Patient> Patients
		{
			get { return patients; }
		}

		public IReadOnlyList<Appointment> Appointments
		{
			get { return appointments; }
		}
	}
}

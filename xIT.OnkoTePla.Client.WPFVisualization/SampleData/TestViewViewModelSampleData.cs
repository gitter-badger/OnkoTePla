using System.Collections.Generic;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using xIT.OnkoTePla.Contracts;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class TestViewViewModelSampleData : ITestViewViewModel
	{
		public TestViewViewModelSampleData ()
		{
			TherapyPlaces = CommunicationSampleData.MedicalPractice.AllTherapyPlaces;
			Patients = CommunicationSampleData.PatientList;
			Appointments = CommunicationSampleData.Appointments;
		}

		public IReadOnlyList<TherapyPlace> TherapyPlaces { get; private set; }
		public IReadOnlyList<Patient> Patients { get; private set; }
		public IReadOnlyList<Appointment> Appointments { get; private set; }
	}
}

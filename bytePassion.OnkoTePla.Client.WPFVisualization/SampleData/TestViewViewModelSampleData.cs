using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class TestViewViewModelSampleData : ITestViewViewModel
	{
		public TestViewViewModelSampleData ()
		{
			TherapyPlaces = CommunicationSampleData.MedicalPractice.AllTherapyPlaces;
			Patients      = CommunicationSampleData.PatientList;
			Appointments  = CommunicationSampleData.Appointments;
		}

		public IReadOnlyList<TherapyPlace> TherapyPlaces { get; private set; }
		public IReadOnlyList<Patient>      Patients      { get; private set; }
		public IReadOnlyList<Appointment>  Appointments  { get; private set; }
	}
}

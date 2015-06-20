using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	public class AddAppointmentTestViewModelSampleData : IAddAppointmentTestViewModel
	{
		public IEnumerable<MedicalPractice>       MedicalPractices { get; set; }
		public IEnumerable<User>                  Users            { get; set; }
		public IEnumerable<Patient>               Patients         { get; set; }
		public ObservableCollection<TherapyPlace> TherapyPlaces    { get; set; }

		public MedicalPractice SelectedMedicalPractice { get; set; }
		public string          SelectedDateAsString    { get; set; }
		public User            SelectedUser            { get; set; }
		public Patient         SelectedPatient         { get; set; }
		public string          Description             { get; set; }
		public string          StartTimeAsString       { get; set; }
		public string          EndTimeAsString         { get; set; }
		public TherapyPlace    SelectedTherapyPlace    { get; set; }

		public ICommand LoadReadModel  { get; set; }
		public ICommand AddAppointment { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

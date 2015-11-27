using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage
{
	public class SearchPageViewModelSampleData : ISearchPageViewModel
	{
		public SearchPageViewModelSampleData()
		{
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();

			DisplayedAppointments = new ObservableCollection<Appointment>
			{
				new Appointment(new Patient("John Doe", new Date(3,5,1968), true, Guid.Empty, ""), 
								"testApp", 
								new TherapyPlace(Guid.Empty, Guid.Empty, "1"), 
								new Date(21,10,2015), 
								new Time(10,30), 
								new Time(12,45), 
								Guid.Empty),
				new Appointment(new Patient("John Doe", new Date(3,5,1958), true, Guid.Empty, ""),
								"testApp2",
								new TherapyPlace(Guid.Empty, Guid.Empty, "1"),
								new Date(22,10,2015),
								new Time(10,30),
								new Time(12,45),
								Guid.Empty),
				new Appointment(new Patient("John Doe", new Date(3,5,1948), true, Guid.Empty, ""),
								"testApp3",
								new TherapyPlace(Guid.Empty, Guid.Empty, "1"),
								new Date(23,10,2015),
								new Time(10,30),
								new Time(12,45),
								Guid.Empty)
			};

			SelectedPatient = "John Doe";
			ShowPreviousAppointments = true;
		}

		public ICommand DeleteAppointment { get; } = null;
		public ICommand ModifyAppointment { get; } = null;		

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }

		public bool ShowPreviousAppointments { get; set; }

		public string SelectedPatient { get; }

		public ObservableCollection<Appointment> DisplayedAppointments { get; }
		
		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage
{
	public class SearchPageViewModelSampleData : ISearchPageViewModel
	{
		public SearchPageViewModelSampleData()
		{
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();

			DisplayedAppointments = new ObservableCollection<AppointmentTransferData>
			{
				new AppointmentTransferData(Guid.Empty, 
											"testApp", 								
											new Date(21,10,2015), 
											new Time(10,30), 
											new Time(12,45),
											Guid.Empty,
											Guid.Empty),
				new AppointmentTransferData(Guid.Empty,
											"testApp2",								
											new Date(22,10,2015),
											new Time(10,30),
											new Time(12,45),
											Guid.Empty,
											Guid.Empty),
				new AppointmentTransferData(Guid.Empty,
											"testApp3",								
											new Date(23,10,2015),
											new Time(10,30),
											new Time(12,45),
											Guid.Empty,
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

		public ObservableCollection<AppointmentTransferData> DisplayedAppointments { get; }
		
		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}

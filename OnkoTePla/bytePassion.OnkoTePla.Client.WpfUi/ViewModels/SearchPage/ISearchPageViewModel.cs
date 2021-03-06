﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage.Helper;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage
{
	internal interface ISearchPageViewModel : IViewModel
	{
		ICommand DeleteAppointment { get; }
		ICommand ModifyAppointment { get; }		
		
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		bool ShowPreviousAppointments { get; set; }
		bool NoAppointmentsAvailable { get; }
		string SelectedPatient { get; }
		
		ObservableCollection<DisplayAppointmentData> DisplayedAppointments { get; }
	}
}

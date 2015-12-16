using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage
{
    public interface ISearchPageViewModel : IViewModel
	{
		ICommand DeleteAppointment { get; }
		ICommand ModifyAppointment { get; }		
		
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		bool ShowPreviousAppointments { get; set; }

		string SelectedPatient { get; }

		ObservableCollection<Appointment> DisplayedAppointments { get; }
	}
}

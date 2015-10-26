using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage
{
	public interface ISearchPageViewModel : IViewModel
	{
		ICommand DeleteAppointment { get; }
		ICommand ModifyAppointment { get; }

		ICommand ShowPreviousAppoointments { get; }
		ICommand HidePreviousAppoointments { get; }
		
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		string SelectedPatient { get; }

		ObservableCollection<Appointment> DisplayedAppointments { get; }
	}
}

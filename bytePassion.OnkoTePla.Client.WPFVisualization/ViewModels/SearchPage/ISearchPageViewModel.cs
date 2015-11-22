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
		
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		bool ShowPreviousAppointments { get; set; }

		string SelectedPatient { get; }

		ObservableCollection<Appointment> DisplayedAppointments { get; }
	}
}

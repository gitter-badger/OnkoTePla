using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.TestAndOldStuff.AppointmentOverView
{
	interface IAppointmentOverViewModel
	{
		ObservableCollection<Appointment>     Appointments     { get; }
		ObservableCollection<MedicalPractice> MedicalPractices { get;  }	
		
		string          SelectedDateAsString    { set; }
		MedicalPractice SelectedMedicalPractice { set; }

		ICommand LoadReadModel { get; }
	}
}

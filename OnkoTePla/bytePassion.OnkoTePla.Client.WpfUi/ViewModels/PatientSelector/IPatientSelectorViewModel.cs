using bytePassion.OnkoTePla.Contracts.Patients;
using System.Windows.Data;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector
{
    public interface IPatientSelectorViewModel : IViewModel
	{
		CollectionViewSource Patients { get; }

		string SearchFilter { set; }
        Patient SelectedPatient { set; }

		bool ListIsEmpty { get; }
		bool ShowDeceasedPatients { get; set; }
    }
}

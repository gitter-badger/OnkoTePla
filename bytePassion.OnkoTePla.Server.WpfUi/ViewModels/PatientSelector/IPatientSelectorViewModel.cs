using System.Windows.Data;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientSelector
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

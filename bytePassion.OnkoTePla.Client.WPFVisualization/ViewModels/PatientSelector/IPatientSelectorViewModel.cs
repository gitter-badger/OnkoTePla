using System.Windows.Data;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector
{
	public interface IPatientSelectorViewModel : IViewModel
	{
		CollectionViewSource Patients { get; }

		string SearchFilter { set; }
        Patient SelectedPatient { set; }

		bool ListIsEmpty { get; }
	}
}

using System.Collections.Generic;
using System.Windows.Data;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface IPatientSelectorViewModel
	{
		CollectionViewSource Patients { get; }
        IReadOnlyList<Appointment> Appointments { get; }
        PatientListItem SelectedPatient { get; set; }
		bool IsListEmpty { get; }
		string FilterString { set; }
	}
}

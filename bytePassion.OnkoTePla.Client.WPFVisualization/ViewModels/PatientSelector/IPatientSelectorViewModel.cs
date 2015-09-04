using System.Collections.Generic;
using System.Windows.Data;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector
{
	internal interface IPatientSelectorViewModel : IViewModel
	{
		CollectionViewSource Patients { get; }
        IReadOnlyList<Appointment> Appointments { get; }
        PatientListItem SelectedPatient { get; set; }
	}
}

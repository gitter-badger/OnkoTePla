using System.Collections.Generic;
using System.Windows.Data;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel
{
	internal interface IPatientSelectorViewModel : IViewModelBase
	{
		CollectionViewSource Patients { get; }
        IReadOnlyList<Appointment> Appointments { get; }
        PatientListItem SelectedPatient { get; set; }
	}
}

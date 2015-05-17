using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface IPatientSelectorViewModel
	{
		IReadOnlyList<PatientListItem> Patients { get; }
        PatientListItem SelectedPatient { get; set; }
		bool IsListEmpty { get; }
		string FilterString { set; }
	}
}

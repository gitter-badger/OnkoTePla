using System.Collections.Generic;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	internal interface IPatientSelectorViewModel
	{
		IReadOnlyList<PatientListItem> Patients { get; }
        PatientListItem SelectedPatient { get; set; }
		bool IsListEmpty { get; }
		string FilterString { set; }
	}
}

using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class PatientSelectorViewModelSampleData : IPatientSelectorViewModel 
	{
		public PatientSelectorViewModelSampleData()
		{			
			Patients = PatientListItem.ConvertPatientList(CommunicationSampleData.PatientList);

			IsListEmpty = false;
			FilterString = "";
		}

		public IReadOnlyList<PatientListItem> Patients { get; private set; }
		public bool IsListEmpty { get; private set; }
		public string FilterString { get; set; }
	}
}

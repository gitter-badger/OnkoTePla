using System.Collections.Generic;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using xIT.OnkoTePla.Contracts;


namespace xIT.OnkoTePla.Client.WPFVisualization.SampleData
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

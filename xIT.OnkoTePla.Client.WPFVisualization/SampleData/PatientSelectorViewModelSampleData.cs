using System.Collections.Generic;
using System.Linq;
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
		    SelectedPatient = Patients.FirstOrDefault();
			IsListEmpty = false;
			FilterString = "";
		}

        public PatientListItem SelectedPatient { get; set; }
		public IReadOnlyList<PatientListItem> Patients { get; private set; }
		public bool IsListEmpty { get; private set; }
		public string FilterString { get; set; }
	}
}

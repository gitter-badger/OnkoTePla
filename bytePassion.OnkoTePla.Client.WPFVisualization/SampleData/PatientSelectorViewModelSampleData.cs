using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
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

	    public IReadOnlyList<Appointment> Appointments
	    {
	        get
	        {
	            return CommunicationSampleData.Appointments.Where(a => a.Patient.ID == SelectedPatient.Patient.ID).ToList();
	        }
	    }

	    public PatientListItem SelectedPatient { get; set; }
		public IReadOnlyList<PatientListItem> Patients { get; private set; }
		public bool IsListEmpty { get; private set; }
		public string FilterString { get; set; }
	}
}

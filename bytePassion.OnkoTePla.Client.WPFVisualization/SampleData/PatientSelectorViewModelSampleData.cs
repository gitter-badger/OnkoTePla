using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class PatientSelectorViewModelSampleData : IPatientSelectorViewModel 
	{
		public PatientSelectorViewModelSampleData()
		{			
			//Patients = PatientListItem.ConvertPatientList(CommunicationSampleData.PatientList);
		    SelectedPatient = null;
			IsListEmpty = false;
			FilterString = "";
		}

	    public IReadOnlyList<Appointment> Appointments
	    {
	        get
	        {
	            //return CommunicationSampleData.ObservableAppointments.Where(a => a.Patient.Id == SelectedPatient.Patient.Id).ToList();
				return new List<Appointment>();
	        }
	    }

	    public PatientListItem SelectedPatient { get; set; }
		public CollectionViewSource Patients { get; private set; }
		public bool IsListEmpty { get; private set; }
		public string FilterString { get; set; }
	}
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel
{
	internal class PatientSelectorViewModelSampleData : IPatientSelectorViewModel 
	{
		public PatientSelectorViewModelSampleData()
		{			
			//Patients = PatientListItem.ConvertPatientList(CommunicationSampleData.PatientList);
		    SelectedPatient = null;
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

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

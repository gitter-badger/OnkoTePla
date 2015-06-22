using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class PatientSelectorViewModel : IPatientSelectorViewModel, INotifyPropertyChanged
	{
	    private readonly IReadOnlyList<Appointment> appointments;
	    private string filterstring;
		private readonly IPatientReadRepository patients;
		private bool isListEmpty;
        private PatientListItem selectedPatient;
	    private string searchFilter;

	    public PatientSelectorViewModel(IPatientReadRepository patients)
		{
		    Patients = new CollectionViewSource();
	        Patients.Filter += Filter;
		    Patients.Source = PatientListItem.ConvertPatientList(patients.GetAllPatients().ToList());
		}

        public CollectionViewSource Patients { get; set; }

        public string SearchFilter
        {
            get { return searchFilter; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    searchFilter = value;

                }

                Patients.View.Refresh();
            }
        }

        public IReadOnlyList<Appointment> Appointments
	    {
            get
            {
                return null;
            }
	    }

        public PatientListItem SelectedPatient
			{
            get { return selectedPatient; }
            set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); PropertyChanged.Notify(this, "ObservableAppointments");}
		}

	    public bool IsListEmpty { get; }
	    public string FilterString { get; set; }

	    public event PropertyChangedEventHandler PropertyChanged;

        private void Filter(object sender, FilterEventArgs e)
        {
            // see Notes on Filter Methods:
            var src = e.Item as PatientListItem;
            if (string.IsNullOrEmpty(SearchFilter))
                e.Accepted = false;
            else if (!src.Patient.Name.ToLower().Contains(SearchFilter)) 
                e.Accepted = false;
        }
    }
}

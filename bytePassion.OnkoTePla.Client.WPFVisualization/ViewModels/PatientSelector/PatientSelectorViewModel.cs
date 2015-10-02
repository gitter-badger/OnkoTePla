using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector
{
	internal class PatientSelectorViewModel : IPatientSelectorViewModel
    {
        
        private Patient selectedPatient;
        private string searchFilter;

        public PatientSelectorViewModel(IDataCenter dataCenter)
        {	        
            Patients = new CollectionViewSource();
            Patients.Filter += Filter;
            Patients.Source = dataCenter.Patients.GetAllPatients().ToList();

			SearchFilter = "";
		}

        public CollectionViewSource Patients { get; }

        public string SearchFilter
        {
            get { return searchFilter; }
            set
            {
                searchFilter = value;
	            SelectedPatient = null;
                Patients.View.Refresh();
            }
        }
		       
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
	        set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
        }        

        private void Filter(object sender, FilterEventArgs e)
        {
			var patientToCheck = e.Item as Patient;

	        e.Accepted = IsPatientNameWithinFilter(patientToCheck, SearchFilter);			                               
        }

		private static bool IsPatientNameWithinFilter(Patient p, string filter)
		{
			if (string.IsNullOrWhiteSpace(filter))
				return true;

			if (p.Name.ToLower().Contains(filter.ToLower()))
				return true;

			return false;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
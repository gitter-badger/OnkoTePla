using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector
{
	internal class PatientSelectorViewModel : IPatientSelectorViewModel
    {
		private readonly IGlobalState<Patient> selectedPatientGlobalVariable;
		private readonly IReadOnlyList<Patient> allPatients;

		private Patient selectedPatient;
        private string searchFilter;
		private bool listIsEmpty;		

		public PatientSelectorViewModel(IDataCenter dataCenter, IGlobalState<Patient> selectedPatientGlobalVariable)
        {
			this.selectedPatientGlobalVariable = selectedPatientGlobalVariable;
			
			allPatients = dataCenter.Patients.GetAllPatients().ToList();

			Patients = new CollectionViewSource();
            Patients.Filter += Filter;
			Patients.Source = allPatients;

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

				CheckList();
            }
        }
		       
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
	        set
	        {
		        selectedPatientGlobalVariable.Value = value;
		        PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value);
	        }
        }

		public bool ListIsEmpty
		{
			get { return listIsEmpty; }
			private set { PropertyChanged.ChangeAndNotify(this, ref listIsEmpty, value); }
		}

		private void CheckList()
		{			
			var filteredPatients = allPatients.Where(patient => patient.Name.ToLower().Contains(SearchFilter.ToLower())).ToList();

			if (filteredPatients.Count == 1)
				SelectedPatient = filteredPatients[0];

			ListIsEmpty = filteredPatients.Count == 0;
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
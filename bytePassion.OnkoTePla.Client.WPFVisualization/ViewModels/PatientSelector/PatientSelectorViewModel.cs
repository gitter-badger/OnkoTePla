using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector
{
	internal class PatientSelectorViewModel : IPatientSelectorViewModel
    {
        
        private Patient selectedPatient;
        private string searchFilter;
		private bool listIsEmpty;

		private readonly DispatcherTimer uglyHackTimer;
	

		public PatientSelectorViewModel(IDataCenter dataCenter)
        {	        
			uglyHackTimer = new DispatcherTimer				//
			{												//
				IsEnabled = true,
				Interval = new TimeSpan(0,0,0,0,5)			
			};

			uglyHackTimer.Tick += (sender, args) =>
			{
				SearchFilter = "";
				uglyHackTimer.IsEnabled = false;
			};

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

				CheckList();
            }
        }
		       
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
	        set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
        }

		public bool ListIsEmpty
		{
			get { return listIsEmpty; }
			private set { PropertyChanged.ChangeAndNotify(this, ref listIsEmpty, value); }
		}

		private void CheckList()
		{
			ListIsEmpty = Patients.View.IsEmpty;

//			foreach (var VARIABLE in Patients.View.)
//			{
//				
//			}
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
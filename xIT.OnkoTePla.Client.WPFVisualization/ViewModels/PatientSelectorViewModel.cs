using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using xIT.Lib.Notifyer;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using xIT.OnkoTePla.Contracts.Patients;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels
{
    internal class PatientSelectorViewModel : IPatientSelectorViewModel, INotifyPropertyChanged
    {
        private string filterstring;
        private readonly IReadOnlyList<PatientListItem> patients;
        private bool isListEmpty;
        private PatientListItem selectedPatient;

        public PatientSelectorViewModel(IReadOnlyList<Patient> patients)
        {
            this.patients = PatientListItem.ConvertPatientList(patients);
            filterstring = "";

            IsListEmpty = Patients.Count == 0;
        }

        public IReadOnlyList<PatientListItem> Patients
        {
            get { return patients; }
        }

        private static bool IsMatch(string patientName, string filterString)
        {
            var seperatedPatientNames = patientName.Split(' ');

            return seperatedPatientNames.Any(namepart => namepart.ToLower().StartsWith(filterString));
        }

        public bool IsListEmpty
        {
            get { return isListEmpty; }
            private set { PropertyChanged.ChangeAndNotify(this, ref isListEmpty, value); }
        }

        public PatientListItem SelectedPatient
        {
            get { return selectedPatient; }
            set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
        }

        public string FilterString
        {
            set
            {
                var trimmedValue = value.Trim();

                if (String.Equals(filterstring, trimmedValue, StringComparison.CurrentCultureIgnoreCase)) return;
                filterstring = value.ToLower();

                if (String.IsNullOrEmpty(filterstring))
                {
                    foreach (var patientListItem in Patients)
                        patientListItem.IsCurrentVisibleInList = true;
                }

                foreach (var patientListItem in Patients)
                    patientListItem.IsCurrentVisibleInList = IsMatch(patientListItem.Patient.Name, filterstring);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage
{
    public class SearchPageViewModel : ISearchPageViewModel
    {
        private ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();
        //private readonly IPatientReadRepository dataCenter;
        private PatientListItem selectedPatient;
        private string searchFilter;
        private IDataCenter m_dataCenter;

        public SearchPageViewModel(IDataCenter dataCenter)
        {
            Patients = new CollectionViewSource();
            Patients.Filter += Filter;
            Patients.Source = PatientListItem.ConvertPatientList(dataCenter.Patients.GetAllPatients().ToList());
            m_dataCenter = dataCenter;
        }

        public CollectionViewSource Patients { get; set; }

        public string SearchFilter
        {
            get { return searchFilter; }
            set
            {
                searchFilter = value;


                Patients.View.Refresh();
            }
        }

        public ObservableCollection<Appointment> Appointments
        {
            get { return appointments; }
            set
            {
                appointments = value;
                PropertyChanged.Notify(this, nameof(Appointments));
            }
        }

        public PatientListItem SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                selectedPatient = value;
                //PropertyChanged.Notify(this, "SelectedPatient");
                //UpdateAppointmentList(value);
            }
        }

        private void UpdateAppointmentList(PatientListItem patientListItem)
        {
            Appointments.Clear();

            if (patientListItem != null)
            {
                var appointmentsReadmodel =
                    m_dataCenter.ReadModelRepository.GetAppointmentsOfAPatientReadModel(patientListItem.Patient.Id);
                appointmentsReadmodel.Appointments.ToList().ForEach(Appointments.Add);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Filter(object sender, FilterEventArgs e)
        {
            // see Notes on Filter Methods:
            var src = e.Item as PatientListItem;
            if (string.IsNullOrEmpty(SearchFilter))
                e.Accepted = true;
            else if (!src.Patient.Name.ToLower().Contains(SearchFilter))
                e.Accepted = false;
        }
    }
}
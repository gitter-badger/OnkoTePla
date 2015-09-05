using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.TestAndOldStuff.AddAppointmentTestView;
using bytePassion.OnkoTePla.Client.WPFVisualization.TestAndOldStuff.AppointmentOverView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.TestAndOldStuff.MainWindow
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		
		private readonly IPatientSelectorViewModel         patientSelectorViewModel;
		private readonly IAddAppointmentTestViewModel      addAppointmentTestViewModel;
		private readonly IAppointmentOverViewModel         appointmentOverViewModel;
		private readonly IAppointmentGridViewModel         appointmentGridViewModel;
		private readonly IDateSelectorViewModel            dateSelectorViewModel;
		private readonly IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel;
		private readonly IRoomFilterViewModel            roomFilterViewModel;

		private readonly SessionInformation sessionInformation;
		

		public MainWindowViewModel (IPatientSelectorViewModel patientSelectorViewModel,
									IAddAppointmentTestViewModel addAppointmentTestViewModel, 
									IAppointmentOverViewModel appointmentOverViewModel, 
									IAppointmentGridViewModel appointmentGridViewModel,
									IDateSelectorViewModel dateSelectorViewModel,
									IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel,
									IRoomFilterViewModel roomFilterViewModel,
									SessionInformation sessionInformation)
		{
		    this.patientSelectorViewModel         = patientSelectorViewModel;
			this.addAppointmentTestViewModel      = addAppointmentTestViewModel;
			this.appointmentOverViewModel         = appointmentOverViewModel;
			this.appointmentGridViewModel         = appointmentGridViewModel;
			this.dateSelectorViewModel            = dateSelectorViewModel;
			this.medicalPracticeSelectorViewModel = medicalPracticeSelectorViewModel;
			this.roomFilterViewModel            = roomFilterViewModel;

			this.sessionInformation = sessionInformation;
			
		}

		public IPatientSelectorViewModel         PatientSelectorViewModel         { get { return patientSelectorViewModel;         }}
		public IAddAppointmentTestViewModel      AddAppointmentTestViewModel      { get { return addAppointmentTestViewModel;      }}
		public IAppointmentOverViewModel         AppointmentOverViewModel         { get { return appointmentOverViewModel;         }}
		public IAppointmentGridViewModel         AppointmentGridViewModel         { get { return appointmentGridViewModel;         }}
		public IDateSelectorViewModel            DateSelectorViewModel            { get { return dateSelectorViewModel;            }}
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get { return medicalPracticeSelectorViewModel; }}
		public IRoomFilterViewModel            RoomFilterViewModel            { get { return roomFilterViewModel;            }}
	}
}

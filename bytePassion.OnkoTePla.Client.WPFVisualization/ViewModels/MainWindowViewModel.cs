using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		
		private readonly IPatientSelectorViewModel    patientSelectorViewModel;
		private readonly IAddAppointmentTestViewModel addAppointmentTestViewModel;
		private readonly IAppointmentOverViewModel    appointmentOverViewModel;
		private readonly IAppointmentGridViewModel    appointmentGridViewModel;
		private readonly IDateSelectorViewModel       dateSelectorViewModel;

		private readonly SessionInformation sessionInformation;
		

		public MainWindowViewModel (IPatientSelectorViewModel patientSelectorViewModel,
									IAddAppointmentTestViewModel addAppointmentTestViewModel, 
									IAppointmentOverViewModel appointmentOverViewModel, 
									IAppointmentGridViewModel appointmentGridViewModel,
									IDateSelectorViewModel dateSelectorViewModel,
									SessionInformation sessionInformation)
		{
		    this.patientSelectorViewModel    = patientSelectorViewModel;
			this.addAppointmentTestViewModel = addAppointmentTestViewModel;
			this.appointmentOverViewModel    = appointmentOverViewModel;
			this.appointmentGridViewModel    = appointmentGridViewModel;
			this.dateSelectorViewModel       = dateSelectorViewModel;

			this.sessionInformation = sessionInformation;
			
		}

		public IPatientSelectorViewModel    PatientSelectorViewModel    { get { return patientSelectorViewModel;    }}
		public IAddAppointmentTestViewModel AddAppointmentTestViewModel { get { return addAppointmentTestViewModel; }}
		public IAppointmentOverViewModel    AppointmentOverViewModel    { get { return appointmentOverViewModel;    }}
		public IAppointmentGridViewModel    AppointmentGridViewModel    { get { return appointmentGridViewModel;    }}
		public IDateSelectorViewModel       DateSelectorViewModel       { get { return dateSelectorViewModel;       }}
	}
}

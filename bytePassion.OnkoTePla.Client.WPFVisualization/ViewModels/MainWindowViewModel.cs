using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		
		//private readonly IPatientSelectorViewModel    patientSelectorViewModel;
		private readonly IAddAppointmentTestViewModel addAppointmentTestViewModel;
		private readonly IAppointmentOverViewModel appointmentOverViewModel;

		public MainWindowViewModel (/*IPatientSelectorViewModel patientSelectorViewModel, */
									IAddAppointmentTestViewModel addAppointmentTestViewModel, 
									IAppointmentOverViewModel appointmentOverViewModel)
		{
			//this.patientSelectorViewModel = patientSelectorViewModel;
			this.addAppointmentTestViewModel = addAppointmentTestViewModel;
			this.appointmentOverViewModel = appointmentOverViewModel;
		}

		//public IPatientSelectorViewModel PatientSelectorViewModel
		//{
		//	get { return patientSelectorViewModel; }
		//}

		public IAddAppointmentTestViewModel AddAppointmentTestViewModel
		{
			get { return addAppointmentTestViewModel; }
		}

		public IAppointmentOverViewModel AppointmentOverViewModel
		{
			get { return appointmentOverViewModel; }
		}
	}
}

using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		
		private readonly IPatientSelectorViewModel    patientSelectorViewModel;
		private readonly IAddAppointmentTestViewModel addAppointmentTestViewModel;

		public MainWindowViewModel (IPatientSelectorViewModel patientSelectorViewModel, 
									IAddAppointmentTestViewModel addAppointmentTestViewModel)
		{
			this.patientSelectorViewModel = patientSelectorViewModel;
			this.addAppointmentTestViewModel = addAppointmentTestViewModel;
		}

		public IPatientSelectorViewModel PatientSelectorViewModel
		{
			get { return patientSelectorViewModel; }
		}

		public IAddAppointmentTestViewModel AddAppointmentTestViewModel
		{
			get { return addAppointmentTestViewModel; }
		}
	}
}

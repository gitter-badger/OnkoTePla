using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData ()
		{			
			PatientSelectorViewModel    = new PatientSelectorViewModelSampleData();
			AddAppointmentTestViewModel = new AddAppointmentTestViewModelSampleData();
			AppointmentOverViewModel    = new AppointmentOverViewModelSampleData();
			AppointmentGridViewModel    = new AppointmentGridViewModelSampleData();
			DateSelectorViewModel       = new DateSelectorViewModelSampleData();
		}
		
		public IPatientSelectorViewModel    PatientSelectorViewModel    { get; private set; }
		public IAddAppointmentTestViewModel AddAppointmentTestViewModel { get; private set; }
		public IAppointmentOverViewModel    AppointmentOverViewModel    { get; private set; }
		public IAppointmentGridViewModel    AppointmentGridViewModel    { get; private set; }
		public IDateSelectorViewModel       DateSelectorViewModel       { get; private set; }
	}
}

using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData ()
		{			
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();
			AddAppointmentTestViewModel = new AddAppointmentTestViewModelSampleData();
		}
		
		public IPatientSelectorViewModel    PatientSelectorViewModel    { get; private set; }
		public IAddAppointmentTestViewModel AddAppointmentTestViewModel { get; private set; }
	}
}

using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData ()
		{
			TestViewViewModel = new TestViewViewModelSampleData();
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();
		}

		public ITestViewViewModel TestViewViewModel { get; private set; }
		public IPatientSelectorViewModel PatientSelectorViewModel { get; private set; }
	}
}

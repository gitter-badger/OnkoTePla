using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace xIT.OnkoTePla.Client.WPFVisualization.SampleData
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

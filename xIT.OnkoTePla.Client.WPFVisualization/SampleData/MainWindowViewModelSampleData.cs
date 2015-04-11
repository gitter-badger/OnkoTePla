using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace xIT.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			TestViewViewModel = new TestViewViewModelMock();
		}

		public ITestViewViewModel TestViewViewModel { get; private set; }
	}
}

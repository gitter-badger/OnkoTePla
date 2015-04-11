using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		private readonly ITestViewViewModel testViewViewModel;

		public MainWindowViewModel(ITestViewViewModel testViewViewModel)
		{
			this.testViewViewModel = testViewViewModel;
		}

		public ITestViewViewModel TestViewViewModel
		{
			get { return testViewViewModel; }
		}
	}
}

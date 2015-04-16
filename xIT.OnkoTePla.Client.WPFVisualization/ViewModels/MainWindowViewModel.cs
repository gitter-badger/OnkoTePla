using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace xIT.OnkoTePla.Client.WPFVisualization.ViewModels
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		private readonly ITestViewViewModel testViewViewModel;
		private readonly IPatientSelectorViewModel patientSelectorViewModel;

		public MainWindowViewModel (ITestViewViewModel testViewViewModel, IPatientSelectorViewModel patientSelectorViewModel)
		{
			this.testViewViewModel = testViewViewModel;
			this.patientSelectorViewModel = patientSelectorViewModel;
		}

		public ITestViewViewModel TestViewViewModel
		{
			get { return testViewViewModel; }
		}

		public IPatientSelectorViewModel PatientSelectorViewModel
		{
			get { return patientSelectorViewModel; }
		}
	}
}

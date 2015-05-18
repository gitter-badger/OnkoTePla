using System.Windows;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels;
using bytePassion.OnkoTePla.Contracts;


namespace bytePassion.OnkoTePla.Client.WPFVisualization
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////                               Composition Root                              //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////

			

			var testViewViewModel = new TestViewViewModel(CommunicationSampleData.MedicalPractice.AllTherapyPlaces,
														  CommunicationSampleData.PatientList,
														  CommunicationSampleData.Appointments);

			var patientSelectorViewModel = new PatientSelectorViewModel(CommunicationSampleData.PatientList, CommunicationSampleData.Appointments);

			var mainWindowViewModel = new MainWindowViewModel(testViewViewModel, patientSelectorViewModel);


			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}
	}
}

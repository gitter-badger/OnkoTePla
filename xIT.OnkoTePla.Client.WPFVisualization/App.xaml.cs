using System.Windows;
using xIT.OnkoTePla.Client.Core.Communication;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels;
using xIT.OnkoTePla.Contracts.Communication;


namespace xIT.OnkoTePla.Client.WPFVisualization
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


			IAppointmentInfoProvider     appointmentInfo     = new AppointmentDataProviderMock();
			IMedicalPracticeInfoProvider medicalPracticeInfo = new MedicalPracticeInfoProviderMock();
			IPatientInfoProvider         patientInfo         = new PatientDataProviderMock();

			var testViewViewModel = new TestViewViewModel(medicalPracticeInfo.GetMedicalPractice().AllTherapyPlaces,
														  patientInfo.GetPatients(),
														  appointmentInfo.GetAppointments());

			var patientSelectorViewModel = new PatientSelectorViewModel(patientInfo.GetPatients());

			var mainWindowViewModel = new MainWindowViewModel(testViewViewModel, patientSelectorViewModel);


			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			mainWindow.Show();
		}
	}
}

using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.XMLDataStores;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Factorys;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.SampleDataGenerators;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi
{
	public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

			///////////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                                 //////////
			////////                            Composition Root and Setup                           //////////
			////////                                                                                 //////////
			///////////////////////////////////////////////////////////////////////////////////////////////////


			// Patient-Repository

			var patientPersistenceService = new XmlPatientDataStore(GlobalConstants.PatientPersistenceFile);
			var patientRepository = new PatientRepository(patientPersistenceService);
			patientRepository.LoadRepository();


			// Config-Repository

			var configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			var configRepository = new ConfigurationRepository(configPersistenceService);
			configRepository.LoadRepository();

			
			var persistenceService = new XmlEventStreamDataStore(GlobalConstants.EventHistoryPersistenceFile);
			//var streamPersistenceService = new StreamPersistenceService(configRepository, "");
			//var streamManager = new StreamManagementService(streamPersistenceService);
			var eventStore = new EventStore(persistenceService, /*streamManager,*/ configRepository);
			eventStore.LoadRepository();


			// DataAndService

			var dataCenterBuilder = new DataCenterBuilder(patientRepository, configRepository, eventStore);
			var dataCenter = dataCenterBuilder.Build();					

	        var connectionServiceBuilder = new ConnectionServiceBuilder(dataCenter, eventStore); 
			var connectionService = connectionServiceBuilder.Build();


			// ViewModel-Variables

			var selectedPageVariable = new SharedState<MainPage>(MainPage.Overview);


			// sampleData-generators

			var patientNameGenerator = new PatientNameGenerator();
			var appointmentGenerator = new AppointmentGenerator(configRepository, patientRepository, eventStore);


			// ViewModels

			var selectedPatientVariable = new SharedState<Patient>(null);
			var patientSelectorViewModel = new PatientSelectorViewModel(patientRepository, selectedPatientVariable);

            var overviewPageViewModel          = new OverviewPageViewModel();
            var connectionsPageViewModel       = new ConnectionsPageViewModel(dataCenter, connectionService);
            var userPageViewModel              = new UserPageViewModel(dataCenter, selectedPageVariable);
            var licencePageViewModel           = new LicencePageViewModel();
            var infrastructurePageViewModel    = new InfrastructurePageViewModel(dataCenter, selectedPageVariable, appointmentGenerator);
			var hoursOfOpeningPageViewModel    = new HoursOfOpeningPageViewModel(dataCenter, selectedPageVariable);
			var therapyPlaceTypesPageViewModel = new TherapyPlaceTypesPageViewModel(dataCenter);
			var patientsPageViewModel          = new PatientsPageViewModel(patientSelectorViewModel, patientRepository, selectedPatientVariable, patientNameGenerator);
			var optionsPageViewModel           = new OptionsPageViewModel();
            var aboutPageViewModel             = new AboutPageViewModel();

	        
	        var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
                                                              connectionsPageViewModel,
                                                              userPageViewModel,
                                                              licencePageViewModel,
                                                              infrastructurePageViewModel, 
															  hoursOfOpeningPageViewModel,  
															  therapyPlaceTypesPageViewModel,  
															  patientsPageViewModel,                                                         
                                                              optionsPageViewModel,
                                                              aboutPageViewModel,
															  selectedPageVariable);
            var mainWindow = new MainWindow
                             {
                                 DataContext = mainWindowViewModel
                             };

            mainWindow.ShowDialog();

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ////////                                                                             //////////
            ////////             Clean Up and store data after main Window was closed            //////////
            ////////                                                                             //////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            
			dataCenterBuilder.PersistConfigRepostiory();
			dataCenterBuilder.PersistPatientRepository();
			eventStore.PersistRepository();

			connectionServiceBuilder.DisposeConnectionService(connectionService);
        }
    }
}

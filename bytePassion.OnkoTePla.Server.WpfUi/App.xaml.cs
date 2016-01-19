using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.XMLDataStores;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Factorys;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
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

			// Config-Repository

			var configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			var configRepository = new ConfigurationRepository(configPersistenceService);
			configRepository.LoadRepository();


			// DataAndService

			var dataCenterBuilder = new DataCenterBuilder(configRepository, configRepository);			
			var dataCenter = dataCenterBuilder.Build();
		
			var connectionServiceBuilder = new ConnectionServiceBuilder(dataCenter);
			var connectionService = connectionServiceBuilder.Build();


			// ViewModel-Variables

			var selectedPageVariable = new GlobalState<MainPage>(MainPage.Overview);

			// ViewModels

            var overviewPageViewModel          = new OverviewPageViewModel();
            var connectionsPageViewModel       = new ConnectionsPageViewModel(dataCenter, connectionService);
            var userPageViewModel              = new UserPageViewModel(dataCenter, selectedPageVariable);
            var licencePageViewModel           = new LicencePageViewModel();
            var infrastructurePageViewModel    = new InfrastructurePageViewModel(dataCenter, selectedPageVariable);
			var hoursOfOpeningPageViewModel    = new HoursOfOpeningPageViewModel(dataCenter, selectedPageVariable);
			var therapyPlaceTypesPageViewModel = new TherapyPlaceTypesPageViewModel(dataCenter);
			var optionsPageViewModel           = new OptionsPageViewModel();
            var aboutPageViewModel             = new AboutPageViewModel();

	        
	        var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
                                                              connectionsPageViewModel,
                                                              userPageViewModel,
                                                              licencePageViewModel,
                                                              infrastructurePageViewModel, 
															  hoursOfOpeningPageViewModel,  
															  therapyPlaceTypesPageViewModel,                                                           
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
            
			configRepository.PersistRepository();

			connectionServiceBuilder.DisposeConnectionService(connectionService);
        }
    }
}

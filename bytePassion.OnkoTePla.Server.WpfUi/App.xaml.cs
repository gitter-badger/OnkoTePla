using System.Windows;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.XMLDataStores;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Factorys;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi
{
	public partial class App : Application
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
			var connectionServiceBuilder = new ConnectionServiceBuilder();

			var dataCenter = dataCenterBuilder.Build();
			var connectionService = connectionServiceBuilder.Build();


			// ViewModels

            var overviewPageViewModel          = new OverviewPageViewModel();
            var connectionsPageViewModel       = new ConnectionsPageViewModel(dataCenter, connectionService);
            var userPageViewModel              = new UserPageViewModel(dataCenter);
            var licencePageViewModel           = new LicencePageViewModel();
            var infrastructurePageViewModel    = new InfrastructurePageViewModel(dataCenter);
			var therapyPlaceTypesPageViewModel = new TherapyPlaceTypesPageViewModel(dataCenter);
			var optionsPageViewModel           = new OptionsPageViewModel();
            var aboutPageViewModel             = new AboutPageViewModel();
	        
	        var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
                                                              connectionsPageViewModel,
                                                              userPageViewModel,
                                                              licencePageViewModel,
                                                              infrastructurePageViewModel,   
															  therapyPlaceTypesPageViewModel,                                                           
                                                              optionsPageViewModel,
                                                              aboutPageViewModel);
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

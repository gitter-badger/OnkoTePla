using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using System.Windows;


namespace bytePassion.OnkoTePla.Server.WpfUi
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ////////                                                                             //////////
            ////////                          Composition Root and Setup                         //////////
            ////////                                                                             //////////
            ///////////////////////////////////////////////////////////////////////////////////////////////

            var overviewPageViewModel = new OverviewPageViewModel();
            var connectionsPageViewModel = new ConnectionsPageViewModel();
            var licencePageViewModel = new LicencePageViewModel();
            var infrastructurePageViewModel = new InfrastructurePageViewModel();
            var optionsPageViewModel = new OptionsPageViewModel();

            var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
                                                              connectionsPageViewModel,
                                                              licencePageViewModel,
                                                              infrastructurePageViewModel,                                                              
                                                              optionsPageViewModel);
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
        }
    }
}

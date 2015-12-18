﻿using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;
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
            var userPageViewModel = new UserPageViewModel();
            var licencePageViewModel = new LicencePageViewModel();
            var infrastructurePageViewModel = new InfrastructurePageViewModel();
            var optionsPageViewModel = new OptionsPageViewModel();
            var aboutPageViewModel = new AboutPageViewModel();

            var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
                                                              connectionsPageViewModel,
                                                              userPageViewModel,
                                                              licencePageViewModel,
                                                              infrastructurePageViewModel,                                                              
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
        }
    }
}
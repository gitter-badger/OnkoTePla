using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Readmodels;
using System;
using bytePassion.OnkoTePla.Client.WpfUi.Views;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
    internal class MainWindowBuilder : IWindowBuilder<MainWindow>
	{		
		private readonly IDataCenter dataCenter;
        private readonly IViewModelCommunication viewModelCommunication;
        private readonly ICommandBus commandBus;
		private readonly SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory;
        private readonly string versionNumber;

        public MainWindowBuilder(IDataCenter dataCenter,
                                 IViewModelCommunication viewModelCommunication,
								 ICommandBus commandBus,
								 SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory,
                                 string versionNumber)
		{			
			this.dataCenter = dataCenter;
		    this.viewModelCommunication = viewModelCommunication;
		    this.commandBus = commandBus;
			this.sessionAndUserSpecificEventHistory = sessionAndUserSpecificEventHistory;
            this.versionNumber = versionNumber;
		}

		public MainWindow BuildWindow()
		{
            // build modules

            var adornerControl = new AdornerControl();

            // build viewModels

            var mainViewModelBuilder = new MainViewModelBuilder(dataCenter, 
                                                                viewModelCommunication, 
                                                                commandBus, 
                                                                sessionAndUserSpecificEventHistory,
                                                                adornerControl);

            var loginViewModelBuilder = new LoginViewModelBuilder();

            var notificationServiceContainerViewModel = new NotificationServiceContainerViewModel(viewModelCommunication);

		    var connectionStatusViewModel = new ConnectionStatusViewModel();

		    var dialogBuilder = new AboutDialogWindowBuilder(versionNumber);

		    var actionBarViewModel = new ActionBarViewModel(connectionStatusViewModel,
                                                            viewModelCommunication,
                                                            dialogBuilder);

		    var mainWindowViewModel = new MainWindowViewModel(mainViewModelBuilder,
                                                              loginViewModelBuilder,
                                                              notificationServiceContainerViewModel,
                                                              actionBarViewModel);

            // build mainWindow

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

            // set GridContainer as ReferenceElement of AdornerControl

		    adornerControl.ReferenceElement = mainWindow.MainView.OverviewPage.GridContainer;

            viewModelCommunication.RegisterViewModelMessageHandler<ShowDisabledOverlay>(mainWindowViewModel);
            viewModelCommunication.RegisterViewModelMessageHandler<HideDisabledOverlay>(mainWindowViewModel);

            return mainWindow;
		}

		public void DisposeWindow(MainWindow buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

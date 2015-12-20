using System;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class MainWindowBuilder : IWindowBuilder<MainWindow>
	{		
		private readonly IDataCenter dataCenter;
        private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISession session;			    	
        private readonly string versionNumber;

        public MainWindowBuilder(IDataCenter dataCenter,
                                 IViewModelCommunication viewModelCommunication,
								 ISession session,								
                                 string versionNumber)
		{			
			this.dataCenter = dataCenter;
		    this.viewModelCommunication = viewModelCommunication;
	        this.session = session;	        
            this.versionNumber = versionNumber;
		}

		public MainWindow BuildWindow()
		{
            // build modules

            var adornerControl = new AdornerControl();			

            // build viewModels

            var mainViewModelBuilder = new MainViewModelBuilder(dataCenter, 
                                                                viewModelCommunication,
																session,                                                                
                                                                adornerControl);

            var loginViewModelBuilder = new LoginViewModelBuilder(session);

            var notificationServiceContainerViewModel = new NotificationServiceContainerViewModel(viewModelCommunication);

		    var connectionStatusViewModel = new ConnectionStatusViewModel(session);

		    var dialogBuilder = new AboutDialogWindowBuilder(versionNumber);

		    var actionBarViewModel = new ActionBarViewModel(session,
															connectionStatusViewModel,
                                                            viewModelCommunication,
                                                            dialogBuilder);

		    var mainWindowViewModel = new MainWindowViewModel(mainViewModelBuilder,
                                                              loginViewModelBuilder,
                                                              notificationServiceContainerViewModel,
                                                              actionBarViewModel,
															  session);

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

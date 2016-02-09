using System;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.LocalSettings;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.TherapyPlaceTypeRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Core.CommandSystem;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class MainWindowBuilder : IWindowBuilder<MainWindow>
	{
		private readonly ILocalSettingsRepository localSettingsRepository;
		private readonly IClientPatientRepository patientRepository;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IClientTherapyPlaceTypeRepository therapyPlaceTypeRepository;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly ICommandBus commandBus;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISession session;			    	
        private readonly string versionNumber;
		
        public MainWindowBuilder(ILocalSettingsRepository localSettingsRepository,
								 IClientPatientRepository patientRepository,
								 IClientMedicalPracticeRepository medicalPracticeRepository,
								 IClientTherapyPlaceTypeRepository therapyPlaceTypeRepository,
								 IClientReadModelRepository readModelRepository,
								 ICommandBus commandBus,
                                 IViewModelCommunication viewModelCommunication,
								 ISession session,								
                                 string versionNumber)
		{
	        this.localSettingsRepository = localSettingsRepository;
	        this.patientRepository = patientRepository;
	        this.medicalPracticeRepository = medicalPracticeRepository;
	        this.therapyPlaceTypeRepository = therapyPlaceTypeRepository;
	        this.readModelRepository = readModelRepository;
	        this.commandBus = commandBus;
	        this.viewModelCommunication = viewModelCommunication;
	        this.session = session;	        
            this.versionNumber = versionNumber;
		}

		public MainWindow BuildWindow(Action<string> errorCallback)
		{
            // build modules

            var adornerControl = new AdornerControl();			

            // build viewModels

            var mainViewModelBuilder = new MainViewModelBuilder(medicalPracticeRepository,readModelRepository,patientRepository,commandBus, localSettingsRepository, 
                                                                viewModelCommunication,
																session,                                                                
                                                                adornerControl);

            var loginViewModelBuilder = new LoginViewModelBuilder(session, 
																  localSettingsRepository);

            var notificationServiceContainerViewModel = new NotificationServiceContainerViewModel(viewModelCommunication);

		    var connectionStatusViewModel = new ConnectionStatusViewModel(session);

		    var dialogBuilder = new AboutDialogWindowBuilder(versionNumber);

		    var actionBarViewModel = new ActionBarViewModel(session,
															connectionStatusViewModel,
                                                            viewModelCommunication,
                                                            dialogBuilder,
															errorCallback);

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

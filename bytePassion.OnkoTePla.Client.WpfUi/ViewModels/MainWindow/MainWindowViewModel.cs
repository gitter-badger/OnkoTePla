using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, 
                                         IMainWindowViewModel
    {
        private readonly IMainViewModelBuilder  mainViewModelBuilder;
        private readonly ILoginViewModelBuilder loginViewModelBuilder;
	    private readonly ISession               session;

	    private IMainViewModel mainViewModel;        
        private ILoginViewModel loginViewModel;

        private bool isMainViewVisible;
        private bool isLoginViewVisible;
        private bool isDisabledOverlayVisible;


        public MainWindowViewModel(IMainViewModelBuilder mainViewModelBuilder, 
                                   ILoginViewModelBuilder loginViewModelBuilder,
                                   INotificationServiceContainerViewModel notificationServiceContainerViewModel, 
                                   IActionBarViewModel actionBarViewModel,
								   ISession session)
        {
            this.mainViewModelBuilder = mainViewModelBuilder;
            this.loginViewModelBuilder = loginViewModelBuilder;
	        this.session = session;

	        NotificationServiceContainerViewModel = notificationServiceContainerViewModel;
            ActionBarViewModel = actionBarViewModel;

            MainViewModel = mainViewModelBuilder.Build();
            LoginViewModel = loginViewModelBuilder.Build();

            IsMainViewVisible = true;
            IsLoginViewVisible = false;
        }


        public IMainViewModel MainViewModel
        {
            get { return mainViewModel; }
            private set { PropertyChanged.ChangeAndNotify(this, ref mainViewModel, value); }
        }

        public bool IsMainViewVisible
        {
            get { return isMainViewVisible; }
            private set { PropertyChanged.ChangeAndNotify(this, ref isMainViewVisible, value); }
        }

        public bool IsLoginViewVisible
        {
            get { return isLoginViewVisible; }
            private set { PropertyChanged.ChangeAndNotify(this, ref isLoginViewVisible, value); }
        }

        public bool IsDisabledOverlayVisible
        {
            get { return isDisabledOverlayVisible; }
            private set { PropertyChanged.ChangeAndNotify(this, ref isDisabledOverlayVisible, value); }
        }

        public ILoginViewModel LoginViewModel
        {
            get { return loginViewModel; }
            private set { PropertyChanged.ChangeAndNotify(this, ref loginViewModel, value); }
        }

        public IActionBarViewModel ActionBarViewModel { get; }

        public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }

        public void Process(ShowDisabledOverlay message)
        {
            IsDisabledOverlayVisible = true;
        }

        public void Process(HideDisabledOverlay message)
        {
            IsDisabledOverlayVisible = false;
        }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}
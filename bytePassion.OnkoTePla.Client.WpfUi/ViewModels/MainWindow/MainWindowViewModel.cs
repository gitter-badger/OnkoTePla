using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.LoginViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModel : ViewModel, 
                                         IMainWindowViewModel
    {
        private readonly IMainViewModelBuilder mainViewModelBuilder;
        private readonly ILoginViewModelBuilder loginViewModelBuilder;

        private IMainViewModel mainViewModel;        
        private ILoginViewModel loginViewModel;

        private bool isMainViewVisible;
        private bool isLoginViewVisible;


        public MainWindowViewModel(IMainViewModelBuilder mainViewModelBuilder, 
                                   ILoginViewModelBuilder loginViewModelBuilder,
                                   INotificationServiceContainerViewModel notificationServiceContainerViewModel)
        {
            this.mainViewModelBuilder = mainViewModelBuilder;
            this.loginViewModelBuilder = loginViewModelBuilder;
            NotificationServiceContainerViewModel = notificationServiceContainerViewModel;

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

        public ILoginViewModel LoginViewModel
        {
            get { return loginViewModel; }
            private set { PropertyChanged.ChangeAndNotify(this, ref loginViewModel, value); }
        }

        public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			MainViewModel = new MainViewModelSampleData();

		    IsMainViewVisible = true;
		    IsLoginViewVisible = true;

            LoginViewModel = new LoginViewModelSampleData();
            ActionBarViewModel = new ActionBarViewModelSampleData();
            NotificationServiceContainerViewModel = new NotificationServiceContainerViewModelSampleData();
		}
					            
        public IMainViewModel MainViewModel { get; }
        public bool IsMainViewVisible { get; }
        public bool IsLoginViewVisible { get; }

        public ILoginViewModel LoginViewModel { get; }
        public IActionBarViewModel ActionBarViewModel { get; }
        public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

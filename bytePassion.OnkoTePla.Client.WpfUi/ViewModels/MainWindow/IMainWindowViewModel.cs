using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : IViewModel									          
	{
		IMainViewModel MainViewModel { get; }

        bool IsMainViewVisible { get; }
        bool IsLoginViewVisible { get; }

        ILoginViewModel LoginViewModel { get; }
        INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }        
    }
}

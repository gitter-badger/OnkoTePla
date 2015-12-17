using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : IViewModel									          
	{
		IMainViewModel MainViewModel { get; }

        bool IsMainViewVisible { get; }

        INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }
    }
}

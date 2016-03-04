using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : IViewModel, 
                                              IViewModelMessageHandler<ShowDisabledOverlay>,
                                              IViewModelMessageHandler<HideDisabledOverlay>
    {
		IMainViewModel MainViewModel { get; }

        bool IsMainViewVisible { get; }
        bool IsLoginViewVisible { get; }

        bool IsDisabledOverlayVisible { get; }
        ISession Session { get; }


        ILoginViewModel LoginViewModel { get; }
        IActionBarViewModel ActionBarViewModel { get; }
        INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }        
    }
}

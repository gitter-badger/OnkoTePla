using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationView;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer
{

    public interface INotificationServiceContainerViewModel : IViewModel,
															  IViewModelMessageHandler<ShowNotification>,
															  IViewModelMessageHandler<HideNotification>
	{
		ObservableCollection<INotificationViewModel> CurrentVisibleNotifications { get; } 
	}
}
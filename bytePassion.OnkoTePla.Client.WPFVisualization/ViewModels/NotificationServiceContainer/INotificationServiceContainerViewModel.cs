using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer
{

	public interface INotificationServiceContainerViewModel : IViewModel,
															  IViewModelMessageHandler<ShowNotification>,
															  IViewModelMessageHandler<HideNotification>
	{
		ObservableCollection<INotificationViewModel> CurrentVisibleNotifications { get; } 
	}
}
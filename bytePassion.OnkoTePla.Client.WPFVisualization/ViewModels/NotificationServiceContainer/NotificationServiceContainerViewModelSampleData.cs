using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer
{

	public class NotificationServiceContainerViewModelSampleData : INotificationServiceContainerViewModel
	{
		public NotificationServiceContainerViewModelSampleData()
		{
			CurrentVisibleNotifications = new ObservableCollection<INotificationViewModel>
			                              {
				                              new NotificationViewModelSampleData(),
				                              new NotificationViewModelSampleData(),
				                              new NotificationViewModelSampleData()
			                              };
		}

		public void Process(ShowNotification message) {}
		public void Process(HideNotification message) {}

		public ObservableCollection<INotificationViewModel> CurrentVisibleNotifications { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
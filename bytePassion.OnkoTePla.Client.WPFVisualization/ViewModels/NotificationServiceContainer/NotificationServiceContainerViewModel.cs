using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer
{
	public class NotificationServiceContainerViewModel : INotificationServiceContainerViewModel
	{

		private readonly IViewModelCommunication viewModelCommunication;

		private readonly IDictionary<Guid, INotificationViewModel> notifications; 

		public NotificationServiceContainerViewModel(IViewModelCommunication viewModelCommunication)
		{
			this.viewModelCommunication = viewModelCommunication;

			CurrentVisibleNotifications = new ObservableCollection<INotificationViewModel>();
			notifications = new Dictionary<Guid, INotificationViewModel>();	
			
			viewModelCommunication.RegisterViewModelMessageHandler<ShowNotification>(this);
			viewModelCommunication.RegisterViewModelMessageHandler<HideNotification>(this);
		}

		public void Process(ShowNotification message)
		{
			var notificationId = Guid.NewGuid();
			var notificationViewModel = new NotificationViewModel(message.NotificationMessage, 
																  notificationId, 
																  viewModelCommunication);
			
			notifications.Add(notificationId, notificationViewModel);
			CurrentVisibleNotifications.Add(notificationViewModel);
		}

		public void Process(HideNotification message)
		{
			var notificationViewModelToHide = notifications[message.NotificationId];
			CurrentVisibleNotifications.Remove(notificationViewModelToHide);
		}

		public ObservableCollection<INotificationViewModel> CurrentVisibleNotifications { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}

}

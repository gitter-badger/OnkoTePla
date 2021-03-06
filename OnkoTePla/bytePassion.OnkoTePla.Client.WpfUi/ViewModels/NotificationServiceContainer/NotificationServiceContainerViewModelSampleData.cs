﻿using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationView;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer
{

	internal class NotificationServiceContainerViewModelSampleData : INotificationServiceContainerViewModel
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

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;	    
	}
}
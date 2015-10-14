﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer
{

	public class NotificationServiceContainerViewModelSampleData : INotificationServiceContainerViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void Process(ShowNotification message)
		{
			throw new System.NotImplementedException();
		}

		public void Process(HideNotification message)
		{
			throw new System.NotImplementedException();
		}

		public ObservableCollection<INotificationViewModel> CurrentVisibleNotifications { get; }
	}

}
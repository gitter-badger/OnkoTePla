using System;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView
{
	public class NotificationViewModel : INotificationViewModel
	{
		
		public NotificationViewModel(string message,
									 Guid notificationId,
									 IViewModelCommunication viewModelCommunication)
		{			
			Message = message;

			HideNotification = new Command(() =>
			{
				viewModelCommunication.Send(new HideNotification(notificationId));	                               
			});
		}

		public string   Message          { get; }
		public ICommand HideNotification { get; }
	}

}

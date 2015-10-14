using System;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView
{
	public class NotificationViewModel : INotificationViewModel
	{
		private readonly Guid notificationId;

		public NotificationViewModel(string message,
									 Guid notificationId,
									 IViewModelCommunication viewModelCommunication)
		{
			this.notificationId = notificationId;
			Message = message;

			HideNotification = new Command(() =>
			{
				viewModelCommunication.Send(new HideNotification(notificationId));	                               
			});
		}

		public string Message { get; }
		public ICommand HideNotification { get; }
	}

}

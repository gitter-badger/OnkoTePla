using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView
{
	public class NotificationViewModel : INotificationViewModel
	{
		public NotificationViewModel(string message,
									 IViewModelCommunication viewModelCommunication)
		{
			Message = message;

			HideNotification = new Command(() =>
			{
				viewModelCommunication.Send(new HideNotification());	                               
			});
		}

		public string Message { get; }
		public ICommand HideNotification { get; }
	}

}

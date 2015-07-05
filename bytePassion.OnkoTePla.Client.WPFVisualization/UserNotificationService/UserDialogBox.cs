using System.Windows;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService
{
	// TODO : replace with mahapps dialog

	public class UserDialogBox
	{
		private readonly string caption;
		private readonly string messageText;
		private readonly MessageBoxButton buttons;
		private readonly MessageBoxImage image;

		public UserDialogBox(string caption, string messageText, MessageBoxButton buttons, MessageBoxImage image)
		{
			this.caption = caption;
			this.messageText = messageText;
			this.buttons = buttons;
			this.image = image;
		}

		public MessageBoxResult ShowDialog()
		{
			return MessageBox.Show(messageText, caption, buttons, image);
		}
	}
}

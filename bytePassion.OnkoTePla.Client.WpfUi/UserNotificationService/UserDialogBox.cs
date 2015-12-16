using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;


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

		public async Task<MessageDialogResult> ShowMahAppsDialog()
		{
			//return MessageBox.Show(messageText, caption, buttons, image);
		    return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(caption, messageText, MessageDialogStyle.AffirmativeAndNegative);
        }
	}
}

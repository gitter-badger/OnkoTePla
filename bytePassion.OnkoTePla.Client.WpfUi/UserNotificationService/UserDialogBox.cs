using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.UserNotificationService
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

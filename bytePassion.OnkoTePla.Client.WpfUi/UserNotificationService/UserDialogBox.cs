using System;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;


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
			MessageDialogStyle messageDialogStyle;

			switch (buttons)
			{				
				case MessageBoxButton.OK:          messageDialogStyle = MessageDialogStyle.Affirmative;            break;
				case MessageBoxButton.OKCancel:    messageDialogStyle = MessageDialogStyle.AffirmativeAndNegative; break;
				 
				default: throw new ArgumentException("there buttons are not supported!");
			}

			return await ((MetroWindow)Application.Current.MainWindow).ShowMessageAsync(caption, messageText, messageDialogStyle);
		}
	}
}


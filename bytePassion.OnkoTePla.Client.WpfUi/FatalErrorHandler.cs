using System;
using System.Linq;
using System.Windows;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;

namespace bytePassion.OnkoTePla.Client.WpfUi
{
	internal class FatalErrorHandler
	{
		private readonly ISession session;

		public FatalErrorHandler(ISession session)
		{
			this.session = session;
		}

		public void HandleFatalError(string errorMsg)
		{
			MessageBox.Show($"A fatal error Occured");

			switch (session.CurrentApplicationState)
			{
				case ApplicationState.LoggedIn:
				{
					session.Logout(
						() =>
						{
							session.TryDisconnect(() => { },
								_ => KillWindow()
							);
						},
						_ => KillWindow()
					);
					break;
				}

				case ApplicationState.ConnectedButNotLoggedIn:
				{
					session.TryDisconnect(
						() => {	},
						_ => KillWindow()
					);
					break;
				}

				default:
				{					
					KillWindow();
					break;
				}
			}
		}

		private static void KillWindow ()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var windows = Application.Current.Windows
											 .OfType<WpfUi.MainWindow>()
											 .ToList();

				if (windows.Count == 1)
					windows[0].Close();
				else
					throw new Exception("inner error");
			});
		}
	}
}

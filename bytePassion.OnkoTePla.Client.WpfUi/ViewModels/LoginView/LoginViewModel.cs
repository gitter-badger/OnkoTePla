using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Client.WpfUi.UserNotificationService;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal class LoginViewModel : ViewModel, 
                                    ILoginViewModel
	{
		private static readonly Protocol Protocol = new TcpIpProtocol();

		private readonly ISession session;
		private string selectedUserName;
		
		public LoginViewModel(ISession session)
	    {
		    this.session = session;

			Login = new Command(DoLogin);
			Connect = new Command(DoConnect);
			Disconnect = new Command(DoDisconnect);

			AvailableUsers = session.AvailableUsers
									.Select(user => user.Name)
									.ToObservableCollection();

			session.UserListAvailable += OnNewUserListAvailable;
	    }

		private void OnNewUserListAvailable(IReadOnlyList<User> newUserList)
		{
			AvailableUsers.Clear();

			newUserList.Select(user => user.Name)
					   .Do(AvailableUsers.Add);
		}

		private void DoDisconnect()
		{
			session.TryDisconnect();
		}

		private async void DoConnect()
		{
			if (IpV4AddressIdentifier.IsIpV4Address(ServerAddress + ":" + GlobalConstants.TcpIpPort))
			{
				session.TryConnect(new Address(Protocol, IpV4AddressIdentifier.Parse(ServerAddress + ":" + GlobalConstants.TcpIpPort)));
			}
			else
			{
				var dialog = new UserDialogBox("", $"{ServerAddress} ist keine gültige Ip-Adresse",
											   MessageBoxButton.OK, MessageBoxImage.Error);
				await dialog.ShowMahAppsDialog();
			}
		}

		private void DoLogin()
		{
			var selectedUser = session.AvailableUsers.First(user => user.Name == SelectedUserName);

			session.TryLogin(selectedUser, Password);
		}


		public ICommand Login      { get; }
		public ICommand Connect    { get; }
		public ICommand Disconnect { get; }

		public ObservableCollection<string> AvailableUsers { get; }

		public string SelectedUserName
		{
			get { return selectedUserName; }
			set { PropertyChanged.ChangeAndNotify(this, ref selectedUserName, value); }
		}

		public string Password { private get; set; }
		public string ServerAddress { get; set; }

		public bool AutoConnectOnNextStart { get; set; }

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

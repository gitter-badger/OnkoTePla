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
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
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
		private string serverAddress;

		public LoginViewModel(ISession session)
	    {
		    this.session = session;

			Login = new Command(DoLogin,
								IsLoginPossible);

			Connect = new Command(DoConnect,
								  IsConnectPossible);

			Disconnect = new Command(DoDisconnect,
									 IsDisconnectPossible);

			AvailableUsers = session.AvailableUsers
									.Select(user => user.Name)
									.ToObservableCollection();

			session.UserListAvailable       += OnNewUserListAvailable;
			session.ApplicationStateChanged += OnApplicationStateChanged; 
	    }

		

		private void OnApplicationStateChanged(ApplicationState applicationState)
		{
			((Command)Login).RaiseCanExecuteChanged();
			((Command)Connect).RaiseCanExecuteChanged();
			((Command)Disconnect).RaiseCanExecuteChanged();
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
		private bool IsDisconnectPossible ()
		{
			return session.CurrentApplicationState == ApplicationState.ConnectedButNotLoggedIn;
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
		private bool IsConnectPossible ()
		{
			return IpV4AddressIdentifier.IsIpV4Address(ServerAddress) &&
				   session.CurrentApplicationState == ApplicationState.DisconnectedFromServer;
		}

		private void DoLogin()
		{
			var selectedUser = session.AvailableUsers.First(user => user.Name == SelectedUserName);

			session.TryLogin(selectedUser, Password);
		}
		private bool IsLoginPossible ()
		{
			return session.CurrentApplicationState == ApplicationState.ConnectedButNotLoggedIn &&
				   !string.IsNullOrWhiteSpace(SelectedUserName);
		}

		public ICommand Login      { get; }
		public ICommand Connect    { get; }
		public ICommand Disconnect { get; }

		public ObservableCollection<string> AvailableUsers { get; }

		public string SelectedUserName
		{
			get { return selectedUserName; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref selectedUserName, value);
				((Command)Login).RaiseCanExecuteChanged();
			}
		}

		public string Password { private get; set; }

		public string ServerAddress
		{
			get { return serverAddress; }
			set
			{
				serverAddress = value;
				((Command)Connect).RaiseCanExecuteChanged();
			}
		}

		public bool AutoConnectOnNextStart { get; set; }

		protected override void CleanUp()
		{
			session.UserListAvailable       -= OnNewUserListAvailable;			
			session.ApplicationStateChanged -= OnApplicationStateChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

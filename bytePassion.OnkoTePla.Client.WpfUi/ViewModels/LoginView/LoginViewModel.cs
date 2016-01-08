using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Client.WpfUi.UserNotificationService;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal class LoginViewModel : ViewModel, 
                                    ILoginViewModel
	{
		private static readonly Protocol Protocol = new TcpIpProtocol();

		private readonly ISession session;
		private readonly IDataCenter dataCenter;

		private string selectedUserName;
		private string serverAddress;
		private string clientAddress;

		private bool currentlyTryingToConnect;
		private bool autoConnectOnNextStart;

		public LoginViewModel(ISession session,
							  IDataCenter dataCenter)
	    {
		    this.session = session;
			this.dataCenter = dataCenter;

			Login = new Command(DoLogin,
								IsLoginPossible);

			Connect = new Command(DoConnect,
								  IsConnectPossible);

			DebugConnect = new Command(DoDebugConnect,
									   IsConnectPossible);

			Disconnect = new Command(DoDisconnect,
									 IsDisconnectPossible);

			AutoConnectOnNextStart = dataCenter.IsAutoConnectionEnabled;

			AvailableUsers = session.AvailableUsers
									.Select(user => user.Name)
									.ToObservableCollection();

			ClientIpAddresses = IpAddressCatcher.GetAllAvailableLocalIpAddresses()
												.Select(address => address.Identifier.ToString())
												.ToObservableCollection();

			ClientAddress = ClientIpAddresses.First();

			session.UserListAvailable       += OnNewUserListAvailable;
			session.ApplicationStateChanged += OnApplicationStateChanged;

			currentlyTryingToConnect = false;

			if (AutoConnectOnNextStart)
			{
				var clientIpAddress = dataCenter.AutoConnectionClientAddress.ToString();

				if (ClientIpAddresses.Contains(clientIpAddress))
				{
					ClientAddress = clientIpAddress;
					ServerAddress = dataCenter.AutoConnectionServerAddress.ToString();
					DebugConnect.Execute(null);
				}
			}
	    }		

		private async void OnApplicationStateChanged(ApplicationState applicationState)
		{
			if (applicationState == ApplicationState.TryingToConnect)
				currentlyTryingToConnect = true;

			if (applicationState == ApplicationState.DisconnectedFromServer && currentlyTryingToConnect)
			{
				var dialog = new UserDialogBox("", $"Es kann keine Verbindung mit {ServerAddress} hergestellt werden",
											   MessageBoxButton.OK, MessageBoxImage.Error);
				await dialog.ShowMahAppsDialog();				

				currentlyTryingToConnect = false;
			}

			if (applicationState == ApplicationState.ConnectedButNotLoggedIn)
			{
				currentlyTryingToConnect = false;
			}

			((Command)Login).RaiseCanExecuteChanged();
			((Command)Connect).RaiseCanExecuteChanged();
			((Command)DebugConnect).RaiseCanExecuteChanged();
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

		private async void DoDebugConnect ()
		{
			if (AddressIdentifier.IsIpAddressIdentifier(ServerAddress))
			{
				session.TryDebugConnect(new Address(Protocol, AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress)),
								        new Address(Protocol, AddressIdentifier.GetIpAddressIdentifierFromString(ClientAddress)));
			}
			else
			{
				var dialog = new UserDialogBox("", $"{ServerAddress} ist keine gültige Ip-Adresse",
											   MessageBoxButton.OK, MessageBoxImage.Error);
				await dialog.ShowMahAppsDialog();
			}

			SetAutoConnectToSettings();
		}

		private async void DoConnect()
		{
			if (AddressIdentifier.IsIpAddressIdentifier(ServerAddress))
			{
				session.TryConnect(new Address(Protocol, AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress)),
								   new Address(Protocol, AddressIdentifier.GetIpAddressIdentifierFromString(ClientAddress)));
			}
			else
			{
				var dialog = new UserDialogBox("", $"{ServerAddress} ist keine gültige Ip-Adresse",
											   MessageBoxButton.OK, MessageBoxImage.Error);
				await dialog.ShowMahAppsDialog();
			}

			SetAutoConnectToSettings();
		}

		private void SetAutoConnectToSettings()
		{
			if (AutoConnectOnNextStart)
			{
				dataCenter.IsAutoConnectionEnabled = true;
				dataCenter.AutoConnectionServerAddress = AddressIdentifier.GetIpAddressIdentifierFromString(ServerAddress);
				dataCenter.AutoConnectionClientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(ClientAddress);
			}
			else
			{
				dataCenter.IsAutoConnectionEnabled = false;
				dataCenter.AutoConnectionClientAddress = new IpV4AddressIdentifier(127, 0, 0, 1);
				dataCenter.AutoConnectionServerAddress = new IpV4AddressIdentifier(127, 0, 0, 1);
			}
		}

		private bool IsConnectPossible ()
		{
			return AddressIdentifier.IsIpAddressIdentifier(ServerAddress) &&
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

		public ICommand Login        { get; }
		public ICommand Connect      { get; }
		public ICommand DebugConnect { get; }
		public ICommand Disconnect   { get; }

		public ObservableCollection<string> AvailableUsers    { get; }
		public ObservableCollection<string> ClientIpAddresses { get; }

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
				PropertyChanged.ChangeAndNotify(this, ref serverAddress, value);
				((Command)Connect).RaiseCanExecuteChanged();
			}
		}

		public string ClientAddress
		{
			get { return clientAddress; }
			set { PropertyChanged.ChangeAndNotify(this, ref clientAddress, value); }
		}

		public bool AutoConnectOnNextStart
		{
			get { return autoConnectOnNextStart; }
			set { PropertyChanged.ChangeAndNotify(this, ref autoConnectOnNextStart, value); }
		}

		protected override void CleanUp()
		{
			session.UserListAvailable       -= OnNewUserListAvailable;			
			session.ApplicationStateChanged -= OnApplicationStateChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

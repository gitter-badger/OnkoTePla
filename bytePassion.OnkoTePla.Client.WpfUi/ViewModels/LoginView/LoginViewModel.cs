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
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Resources.UserNotificationService;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.LoginView
{
	internal class LoginViewModel : ViewModel, 
                                    ILoginViewModel
	{
		private static readonly Protocol Protocol = new TcpIpProtocol();

		private readonly ISession session;
		private readonly IDataCenter dataCenter;

		private ClientUserData selectedUser;
		private string serverAddress;
		private string clientAddress;

		private bool currentlyTryingToConnect;
		private bool autoConnectOnNextStart;
		private bool areConnectionSettingsVisible;
		private bool isUserListAvailable;

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

			AvailableUsers = new ObservableCollection<ClientUserData>();

			ClientIpAddresses = IpAddressCatcher.GetAllAvailableLocalIpAddresses()
												.Select(address => address.Identifier.ToString())
												.ToObservableCollection();

			ClientAddress = ClientIpAddresses.First();
			
			session.ApplicationStateChanged += OnApplicationStateChanged;

			currentlyTryingToConnect = false;
			IsUserListAvailable = false;
			AreConnectionSettingsVisible = !AutoConnectOnNextStart;

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
				var dialog = new UserDialogBox("", $"Es kann keine Verbindung mit {ServerAddress} hergestellt werden", MessageBoxButton.OK);
				await dialog.ShowMahAppsDialog();				

				currentlyTryingToConnect = false;
				AreConnectionSettingsVisible = true;
				IsUserListAvailable = false;
			}

			if (applicationState == ApplicationState.ConnectedButNotLoggedIn)
			{
				currentlyTryingToConnect = false;
				AreConnectionSettingsVisible = false;

				session.RequestUserList(
					userList => 
					{
						Application.Current.Dispatcher.Invoke(async () =>
						{
							AvailableUsers.Clear();
							userList.Do(userData => AvailableUsers.Add(userData));
							IsUserListAvailable = true;

							if (AvailableUsers.Count > 0)
							{
								SelectedUser = AvailableUsers.First(); // TODO: letzt eingeloggten user wählen
							}
							else
							{
								var dialog = new UserDialogBox("",
															   "Es sind keine verfügbaren User vorhanden\n" +														 
															   "Die Verbindung wird getrennt!",
															   MessageBoxButton.OK);
								await dialog.ShowMahAppsDialog();
							}
						});					
					},
					errorMessage =>
					{
						Application.Current.Dispatcher.Invoke(async () =>
						{
							var dialog = new UserDialogBox("", 
														  "Die Userliste kann nicht vom Server abgefragt werden:\n" +
														  $">> {errorMessage} <<\n" +
														  "Die Verbindung wird getrennt - versuchen Sie es erneut", 
														  MessageBoxButton.OK);
							await dialog.ShowMahAppsDialog();
							Disconnect.Execute(null);
						});
					}
				);
			}

			((Command)Login).RaiseCanExecuteChanged();
			((Command)Connect).RaiseCanExecuteChanged();
			((Command)DebugConnect).RaiseCanExecuteChanged();
			((Command)Disconnect).RaiseCanExecuteChanged();
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
											   MessageBoxButton.OK);
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
											   MessageBoxButton.OK);
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
			if (ServerAddress == null)
				return false;

			return AddressIdentifier.IsIpAddressIdentifier(ServerAddress) &&
				   session.CurrentApplicationState == ApplicationState.DisconnectedFromServer;
		}

		private void DoLogin()
		{			
			session.TryLogin(selectedUser, Password);
		}
		private bool IsLoginPossible ()
		{
			return session.CurrentApplicationState == ApplicationState.ConnectedButNotLoggedIn &&
				   SelectedUser != null;
		}

		public ICommand Login        { get; }
		public ICommand Connect      { get; }
		public ICommand DebugConnect { get; }
		public ICommand Disconnect   { get; }

		public ObservableCollection<ClientUserData> AvailableUsers    { get; }
		public ObservableCollection<string>         ClientIpAddresses { get; }
		
		public ClientUserData SelectedUser
		{
			get { return selectedUser; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref selectedUser, value);
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

		public bool AreConnectionSettingsVisible
		{
			get { return areConnectionSettingsVisible; }
			set { PropertyChanged.ChangeAndNotify(this, ref areConnectionSettingsVisible, value); }
		}

		public bool IsUserListAvailable
		{
			get { return isUserListAvailable; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isUserListAvailable, value); }
		}

		public bool AutoConnectOnNextStart
		{
			get { return autoConnectOnNextStart; }
			set { PropertyChanged.ChangeAndNotify(this, ref autoConnectOnNextStart, value); }
		}

		protected override void CleanUp()
		{			
			session.ApplicationStateChanged -= OnApplicationStateChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage.Helper;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage
{
	internal class ConnectionsPageViewModel : ViewModel, IConnectionsPageViewModel
	{

		private const string NoConnection = "- keine Verbindung momentan -";

	    private readonly IDataCenter dataCenter;
		private readonly IConnectionService connectionService;

		private string selectedIpAddress;
	    private string activeConnection;
		private bool isConnectionActive;
		private bool isActivationPossible;

		private bool connectionActivationLocked;
		private DispatcherTimer lockedTimer;

		public ConnectionsPageViewModel(IDataCenter dataCenter, IConnectionService connectionService)
	    {
		    this.dataCenter = dataCenter;
		    this.connectionService = connectionService;

		    UpdateAvailableAddresses = new Command(UpdateAddresses);

			AvailableIpAddresses = new ObservableCollection<string>();
			ConnectedClients = new ObservableCollection<ConnectedClientDisplayData>();

		    UpdateAddresses();

			ActiveConnection = NoConnection;
			isConnectionActive = false;
			connectionActivationLocked = false;
		    SelectedIpAddress = AvailableIpAddresses.First();

			connectionService.NewSessionStarted += OnNewSessionStarted;
			connectionService.SessionTerminated += OnSessionTerminated;
			connectionService.LoggedInUserUpdated += OnLoggedInUserUpdated;

			CheckIfActicationIsPossible();

			if (IsActivationPossible)
				IsConnectionActive = true; // TODO: just for testing
	    }

		private void OnLoggedInUserUpdated(SessionInfo sessionInfo)
		{
			var displayData = ConnectedClients.First(dd => dd.SessionId == sessionInfo.SessionId.ToString());
			displayData.LogginInUser = sessionInfo.LoggedInUser == null 
											? "no User logged in" 
											: sessionInfo.LoggedInUser.Name;
		}

		private void OnSessionTerminated(SessionInfo sessionInfo)
		{
			var displayData = ConnectedClients.First(dd => dd.SessionId == sessionInfo.SessionId.ToString());
			ConnectedClients.Remove(displayData);
		}

		private void OnNewSessionStarted(SessionInfo sessionInfo)
		{
			ConnectedClients.Add(new ConnectedClientDisplayData(sessionInfo.SessionId.ToString(),
																sessionInfo.CreationTime.ToString(),
																sessionInfo.ClientAddress.ToString()));			
		}

		private void UpdateAddresses()
	    {
			AvailableIpAddresses.Clear();

		    dataCenter.GetAllAvailableAddresses()
					  .Select(address => address.Identifier.ToString())
					  .Do(AvailableIpAddresses.Add);
	    }

	    public ICommand UpdateAvailableAddresses { get; }

		public bool IsConnectionActive
		{
			get { return isConnectionActive; }
			set
			{
				if (value != isConnectionActive)
				{
					if (value)
					{
						ActiveConnection = SelectedIpAddress;
						connectionService.InitiateCommunication(new Address(new TcpIpProtocol(), 
																			AddressIdentifier.GetIpAddressIdentifierFromString(SelectedIpAddress)));
					}
					else
					{
						connectionActivationLocked = true;
						CheckIfActicationIsPossible();
						lockedTimer = new DispatcherTimer
						{
							Interval = TimeSpan.FromSeconds(3),
							IsEnabled = true
						};
						lockedTimer.Tick += OnLockedTimerTick;

						ActiveConnection = NoConnection;
						connectionService.StopCommunication();
					}
				}

				PropertyChanged.ChangeAndNotify(this, ref isConnectionActive, value);
			}
		}

		private void OnLockedTimerTick(object sender, EventArgs eventArgs)
		{
			lockedTimer.Tick -= OnLockedTimerTick;
			lockedTimer.IsEnabled = false;			

			connectionActivationLocked = false;
			CheckIfActicationIsPossible();
		}

		public bool IsActivationPossible
		{
			get { return isActivationPossible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isActivationPossible, value); }
		}

		public string SelectedIpAddress
	    {
		    get { return selectedIpAddress; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref selectedIpAddress, value);
				CheckIfActicationIsPossible();
			}
	    }

	    public string ActiveConnection
	    {
		    get { return activeConnection; }
		    private set { PropertyChanged.ChangeAndNotify(this, ref activeConnection, value); }
	    }

	    public ObservableCollection<string> AvailableIpAddresses { get; }
	    public ObservableCollection<ConnectedClientDisplayData> ConnectedClients { get; }


		private void CheckIfActicationIsPossible()
		{
			IsActivationPossible = !connectionActivationLocked && 
								   !string.IsNullOrWhiteSpace(SelectedIpAddress) &&
								   dataCenter.GetAllUsers().Count(user => !user.IsHidden) > 0 &&
								   dataCenter.GetAllMedicalPractices().Any() &&
								   dataCenter.GetAllTherapyPlaceTypes().Any();								  
		}

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

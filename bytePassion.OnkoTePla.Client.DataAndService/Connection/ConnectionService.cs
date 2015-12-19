using System;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	public class ConnectionService : IConnectionService
	{
		private ConnectionStatus connectionStatus;
		public event Action<ConnectionStatus> ConnectionStatusChanged;


		public ConnectionService()
		{			
			ConnectionStatus = ConnectionStatus.Disconnected;
		}

		public ConnectionStatus ConnectionStatus
		{
			get { return connectionStatus; }
			private set
			{
				connectionStatus = value;
				ConnectionStatusChanged?.Invoke(ConnectionStatus);
			}
		}

		public Address CurrentServerAddress { get; private set; }

        public void TryConnect(Address serverAddress)
        {
	        CurrentServerAddress = serverAddress;

			ConnectionStatus = ConnectionStatus.TryConnect;

			Application.Current.Dispatcher.DelayInvoke(
					() => ConnectionStatus = ConnectionStatus.Connected,
					TimeSpan.FromSeconds(10)
			);
		}

        public void TryDisconnect()
        {
			ConnectionStatus = ConnectionStatus.TryDisconnect;

			Application.Current.Dispatcher.DelayInvoke(
					() => ConnectionStatus = ConnectionStatus.Disconnected,
					TimeSpan.FromSeconds(10)
			);
		}
    }
}

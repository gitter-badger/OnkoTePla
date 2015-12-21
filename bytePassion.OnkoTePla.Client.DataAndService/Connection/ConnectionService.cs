using System;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using NLog;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal class ConnectionService : IConnectionService
	{
		private readonly ILogger logger;

		public event Action<ConnectionEvent> ConnectionEventInvoked;


		public ConnectionService(ILogger logger)
		{
			this.logger = logger;
			ConnectionStatus = ConnectionStatus.Disconnected;
		}

		public ConnectionStatus ConnectionStatus { get; private set; }
		public Address          ServerAddress    { get; private set; }

        public void TryConnect(Address serverAddress)
        {
	        ServerAddress = serverAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);

			Application.Current.Dispatcher.DelayInvoke(
				() =>
				{
					ConnectionStatus = ConnectionStatus.Connected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
				},
				TimeSpan.FromSeconds(2)
			);
		}

        public void TryDisconnect()
        {
			ConnectionStatus = ConnectionStatus.TryingToDisconnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryDisconnect);

			Application.Current.Dispatcher.DelayInvoke(
				() =>
				{
					ConnectionStatus = ConnectionStatus.Disconnected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.Disconnected);					
				},
				TimeSpan.FromSeconds(2)
			);
		}
    }
}

using System;
using System.Threading;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ;
using NLog;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal class ConnectionService : DisposingObject, IConnectionService
	{
		private readonly ILogger logger;
		private readonly NetMQContext zmqContext;

		public event Action<ConnectionEvent> ConnectionEventInvoked;


		public ConnectionService(ILogger logger)
		{
			this.logger = logger;
			ConnectionStatus = ConnectionStatus.Disconnected;

			zmqContext = NetMQContext.Create();
		}

		public ConnectionStatus ConnectionStatus { get; private set; }
		public Address          ServerAddress    { get; private set; }
		public Address			ClientAddress    { get; private set; }

		private ConnectionSessionId CurrentSessionId { get; set; }

		private HeartbeatThead heartbeatThread;
		
        public void TryConnect(Address serverAddress, Address clientAddress)
        {
	        ServerAddress = serverAddress;
	        ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);			

			var threadLogic = new ConnectionBeginThead(zmqContext, serverAddress, clientAddress,ConnectionResponeReceived);
			var runningThread = new Thread(threadLogic.Run);
			runningThread.Start();
		}

		private void ConnectionResponeReceived(ConnectionSessionId connectionSessionId)
		{
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					if (connectionSessionId == null)
					{
						if (ConnectionStatus == ConnectionStatus.TryingToConnect)
						{						
							ConnectionStatus = ConnectionStatus.Disconnected;
							ConnectionEventInvoked?.Invoke(ConnectionEvent.ConAttemptUnsuccessful);																			
						}
					}
					else
					{
						CurrentSessionId = connectionSessionId;

						heartbeatThread = new HeartbeatThead(zmqContext, ClientAddress, CurrentSessionId);
						heartbeatThread.ServerVanished += OnServerVanished;
						var runningThread = new Thread(heartbeatThread.Run);
						runningThread.Start();

						ConnectionStatus = ConnectionStatus.Connected;
						ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
					}					
				}				
			);			
		}

		private void OnServerVanished()
		{
			heartbeatThread.ServerVanished -= OnServerVanished;

			ConnectionStatus = ConnectionStatus.Disconnected;
			Application.Current.Dispatcher.Invoke(() => ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionLost));
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

		protected override void CleanUp()
		{
			zmqContext.Dispose();
		}
	}
}

using System;
using System.Threading;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
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



        public void TryConnect(Address serverAddress, Address clientAddress)
        {
	        ServerAddress = serverAddress;
	        ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);

			Console.WriteLine("tryingToConnect...");

			var threadLogic = new ConnectionThead(zmqContext, serverAddress, clientAddress,ConnectionResponeReceived);
			var runningThread = new Thread(threadLogic.Run);
			runningThread.Start();


			Application.Current.Dispatcher.DelayInvoke(
				() =>
				{
					if (ConnectionStatus == ConnectionStatus.TryingToConnect)
					{
						Console.WriteLine("connection unsuccessful...");

						ConnectionStatus = ConnectionStatus.Disconnected;
						ConnectionEventInvoked?.Invoke(ConnectionEvent.ConAttemptUnsuccessful);

						// TODO: thread abschießen
					}
				},
				TimeSpan.FromSeconds(2)
			);
		}

		private void ConnectionResponeReceived(ConnectionSessionId connectionSessionId)
		{
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					ConnectionStatus = ConnectionStatus.Connected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
				}				
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

		protected override void CleanUp()
		{
			zmqContext.Dispose();
		}
	}
}

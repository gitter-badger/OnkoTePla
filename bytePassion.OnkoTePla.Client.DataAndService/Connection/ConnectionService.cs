using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Contracts.Config;
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

		private bool ConnectionWasTerminated { get; set; }

		private HeartbeatThead heartbeatThread;
		private DataRequestThread dataRequestThread;
		private TimeoutBlockingQueue<RequestObject> requestWorkQueue; 

        public void TryConnect(Address serverAddress, Address clientAddress)
        {
	        ConnectionWasTerminated = false;

			ServerAddress = serverAddress;
	        ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);			

			var threadLogic = new ConnectionBeginThead(zmqContext, serverAddress, 
													   clientAddress, ConnectionBeginResponeReceived);
			var runningThread = new Thread(threadLogic.Run);
			runningThread.Start();
		}

		public void TryDebugConnect(Address serverAddress, Address clientAddress)
		{
			ConnectionWasTerminated = false;

			ServerAddress = serverAddress;
			ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);
			
			var threadLogic = new DebugConnectionBeginThead(zmqContext, serverAddress,
														    clientAddress, DebugConnectionBeginResponeReceived);
			var runningThread = new Thread(threadLogic.Run);
			runningThread.Start();
		}

		public void TryDisconnect ()
		{
			ConnectionStatus = ConnectionStatus.TryingToDisconnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryDisconnect);

			var threadLogic = new ConnectionEndThead(zmqContext, ServerAddress, 
													 CurrentSessionId, ConnectionEndResponseReceived);
			var runningThread = new Thread(threadLogic.Run);
			runningThread.Start();
		}

		public void RequestUserList(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
									Action<string> errorCallback)
		{
			requestWorkQueue.Put(new UserListRequestObject(dataReceivedCallback, errorCallback));
		}

		private void ConnectionEndResponseReceived()
		{
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					ConnectionWasTerminated = true;
					
					ConnectionStatus = ConnectionStatus.Disconnected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.Disconnected);

					CleanUpAfterDisconnection();
				});
		}

		private void ConnectionBeginResponeReceived(ConnectionSessionId connectionSessionId)
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
						new Thread(heartbeatThread.Run).Start();						

						requestWorkQueue = new TimeoutBlockingQueue<RequestObject>(1000);
						dataRequestThread = new DataRequestThread(zmqContext, ServerAddress, requestWorkQueue, CurrentSessionId);
						new Thread(dataRequestThread.Run).Start();
						
						ConnectionStatus = ConnectionStatus.Connected;
						ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
					}					
				}				
			);			
		}

		private void DebugConnectionBeginResponeReceived (ConnectionSessionId connectionSessionId)
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

						requestWorkQueue = new TimeoutBlockingQueue<RequestObject>(1000);
						dataRequestThread = new DataRequestThread(zmqContext, ServerAddress, requestWorkQueue, CurrentSessionId);
						new Thread(dataRequestThread.Run).Start();

						ConnectionStatus = ConnectionStatus.Connected;
						ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
					}
				}
			);
		}

		private void OnServerVanished()
		{
			heartbeatThread.ServerVanished -= OnServerVanished;

			if (!ConnectionWasTerminated)
			{
				ConnectionStatus = ConnectionStatus.Disconnected;
				ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionLost);

				CleanUpAfterDisconnection();
			}
		}

		private void CleanUpAfterDisconnection()
		{
			heartbeatThread?.Stop();
			heartbeatThread = null;

			dataRequestThread.Stop();
			dataRequestThread = null;
			requestWorkQueue = null;
		}

		protected override void CleanUp()
		{
			zmqContext.Dispose();
		}
	}
}

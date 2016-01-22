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
	internal class ConnectionService : DisposingObject, 
									   IConnectionService
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
		

		private bool ConnectionWasTerminated { get; set; }

		private HeartbeatThead heartbeatThread;
		private UniversalRequestThread universalRequestThread;
		private TimeoutBlockingQueue<RequestObject> requestWorkQueue; 
		 
        public void TryConnect(Address serverAddress, Address clientAddress, 
							   Action<string> errorCallback)
        {
			ConnectionWasTerminated = false;

			ServerAddress = serverAddress;
			ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);

			requestWorkQueue = new TimeoutBlockingQueue<RequestObject>(1000);
			universalRequestThread = new UniversalRequestThread(zmqContext, ServerAddress, requestWorkQueue);
			new Thread(universalRequestThread.Run).Start();
			
			requestWorkQueue.Put(new BeginConnectionRequestObject(ConnectionBeginResponeReceived,
																  ClientAddress.Identifier,
																  errorCallback));								
		}

		public void TryDebugConnect(Address serverAddress, Address clientAddress, 
									Action<string> errorCallback)
		{
			ConnectionWasTerminated = false;

			ServerAddress = serverAddress;
			ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);

			requestWorkQueue = new TimeoutBlockingQueue<RequestObject>(1000);
			universalRequestThread = new UniversalRequestThread(zmqContext, ServerAddress, requestWorkQueue);
			new Thread(universalRequestThread.Run).Start();

			requestWorkQueue.Put(new BeginDebugConnectionRequestObject(DebugConnectionBeginResponeReceived,
																	   ClientAddress.Identifier,
																	   errorCallback));
		}

		public void TryDisconnect(Action<string> errorCallback)
		{
			ConnectionStatus = ConnectionStatus.TryingToDisconnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryDisconnect);

			requestWorkQueue.Put(new EndConnectionRequestObject(ConnectionEndResponseReceived,
																errorCallback));
		}

		public void RequestUserList(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
									Action<string> errorCallback)
		{
			requestWorkQueue.Put(new UserListRequestObject(dataReceivedCallback, errorCallback));
		}

		public void TryLogin(Action loginSuccessfulCallback, ClientUserData user, string password, 
							 Action<string> errorCallback)
		{
			requestWorkQueue.Put(new LoginRequestObject(loginSuccessfulCallback, user, password, errorCallback));
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
						heartbeatThread = new HeartbeatThead(zmqContext, ClientAddress, connectionSessionId);
						heartbeatThread.ServerVanished += OnServerVanished;
						new Thread(heartbeatThread.Run).Start();												
						
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

			universalRequestThread?.Stop();
			universalRequestThread = null;
			requestWorkQueue = null;
		}

		protected override void CleanUp()
		{
			zmqContext.Dispose();
		}
	}
}

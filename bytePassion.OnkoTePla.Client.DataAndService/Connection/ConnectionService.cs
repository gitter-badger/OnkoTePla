using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers;
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
		public event Action<ConnectionEvent> ConnectionEventInvoked;

		private readonly ILogger logger;
		private readonly NetMQContext zmqContext;

		private readonly SharedState<ConnectionSessionId> connectionIdVariable;


		public ConnectionService(ILogger logger)
		{
			this.logger = logger;
			ConnectionStatus = ConnectionStatus.Disconnected;

			connectionIdVariable = new SharedState<ConnectionSessionId>(null);
			zmqContext = NetMQContext.Create();
		}

		public ConnectionStatus ConnectionStatus { get; private set; }
		public Address          ServerAddress    { get; private set; }
		public Address			ClientAddress    { get; private set; }
		

		private bool ConnectionWasTerminated { get; set; }

		private HeartbeatThead heartbeatThread;
		private UniversalRequestThread universalRequestThread;
		private TimeoutBlockingQueue<IRequestHandler> requestWorkQueue; 
		 
        public void TryConnect(Address serverAddress, Address clientAddress, 
							   Action<string> errorCallback)
        {
			ConnectionWasTerminated = false;

			ServerAddress = serverAddress;
			ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);
			
			requestWorkQueue = new TimeoutBlockingQueue<IRequestHandler>(1000);
			universalRequestThread = new UniversalRequestThread(zmqContext, ServerAddress, requestWorkQueue);
			new Thread(universalRequestThread.Run).Start();
			
			requestWorkQueue.Put(new BeginConnectionRequestHandler(ConnectionBeginResponeReceived,
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

			requestWorkQueue = new TimeoutBlockingQueue<IRequestHandler>(1000);
			universalRequestThread = new UniversalRequestThread(zmqContext, ServerAddress, requestWorkQueue);
			new Thread(universalRequestThread.Run).Start();

			requestWorkQueue.Put(new BeginDebugConnectionRequestHandler(DebugConnectionBeginResponeReceived,
																	    ClientAddress.Identifier,
																	    errorCallback));
		}

		public void TryDisconnect(Action<string> errorCallback)
		{
			ConnectionStatus = ConnectionStatus.TryingToDisconnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryDisconnect);

			requestWorkQueue.Put(new EndConnectionRequestHandler(ConnectionEndResponseReceived,
																 connectionIdVariable,
																 errorCallback));
		}

		public void RequestUserList(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback,
									Action<string> errorCallback)
		{
			requestWorkQueue.Put(new UserListRequestHandler(dataReceivedCallback, 
															connectionIdVariable, 
															errorCallback));
		}

		public void TryLogin(Action loginSuccessfulCallback, ClientUserData user, string password, 
							 Action<string> errorCallback)
		{
			requestWorkQueue.Put(new LoginRequestHandler(loginSuccessfulCallback, 
														 user, 
														 password, 
														 connectionIdVariable, 
														 errorCallback));
		}

		public void TryLogout(Action logoutSuccessfulCallback, ClientUserData user, Action<string> errorCallback)
		{
			requestWorkQueue.Put(new LogoutRequestHandler(logoutSuccessfulCallback, 
														  user, 
														  connectionIdVariable, 
														  errorCallback));
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
						connectionIdVariable.Value = connectionSessionId;
											
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
						connectionIdVariable.Value = connectionSessionId;

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
			CleanUpAfterDisconnection();
			zmqContext.Dispose();
		}
	}
}

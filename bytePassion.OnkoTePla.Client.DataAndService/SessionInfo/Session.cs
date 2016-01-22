using System;
using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Contracts.Config;
using NLog;


namespace bytePassion.OnkoTePla.Client.DataAndService.SessionInfo
{
	public class Session : ISession
	{
	    private readonly IConnectionService connectionService;
	    private readonly IClientWorkflow clientWorkflow;
		private readonly ILogger logger;

		private ApplicationState currentApplicationState;		    

		internal Session(IConnectionService connectionService, 
						 IClientWorkflow clientWorkflow,
						 ILogger logger)
	    {
		    this.connectionService = connectionService;
		    this.clientWorkflow = clientWorkflow;
			this.logger = logger;

			CurrentApplicationState = clientWorkflow.CurrentState;

			clientWorkflow.StateChanged += OnApplicationStateChanged;
			connectionService.ConnectionEventInvoked += OnConnectionServiceEventInvoked;
		}
		

		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                 applicationState                                  ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		public event Action<ApplicationState> ApplicationStateChanged;

		public ApplicationState CurrentApplicationState
		{
			get { return currentApplicationState; }
			private set
			{
				if (currentApplicationState != value)
				{
					currentApplicationState = value;
					logger.Debug($"CurrentApplicationState changed to: {value}");
					ApplicationStateChanged?.Invoke(CurrentApplicationState);
				}
			}		
		}
		
		private void OnApplicationStateChanged (ApplicationState newApplicationState)
		{
			CurrentApplicationState = newApplicationState;
		}

		private void ApplyWorkflowEvent (WorkflowEvent workflowEvent)
		{
			clientWorkflow.ApplyEvent(workflowEvent);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                     userInfo                                      ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////


		public ClientUserData LoggedInUser { get; private set; }

		public void RequestUserList(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback, 
									Action<string> errorCallback)
		{
			connectionService.RequestUserList(dataReceivedCallback, errorCallback);
		}
		
		public void TryLogin(ClientUserData user, string password, Action<string> errorCallback)
		{
			connectionService.TryLogin(
				() =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						LoggedInUser = user;
						ApplyWorkflowEvent(WorkflowEvent.LoggedIn);
					});
				},
				user,
				password,
				errorCallback	
			);			
		}
		
		public void Logout()
		{
			LoggedInUser = null;
			ApplyWorkflowEvent(WorkflowEvent.LoggedOut);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                 serverConnection                                  ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////


		private void OnConnectionServiceEventInvoked (ConnectionEvent connectionEvent)
		{
			switch (connectionEvent)
			{
				case ConnectionEvent.ConnectionEstablished:  ApplyWorkflowEvent(WorkflowEvent.ConnectionEstablished);  break;
				case ConnectionEvent.Disconnected:           ApplyWorkflowEvent(WorkflowEvent.Disconnected);           break;
				case ConnectionEvent.ConAttemptUnsuccessful: ApplyWorkflowEvent(WorkflowEvent.ConAttemptUnsuccessful); break;
				case ConnectionEvent.ConnectionLost:         ApplyWorkflowEvent(WorkflowEvent.ConnectionLost);         break;
				case ConnectionEvent.StartedTryConnect:      ApplyWorkflowEvent(WorkflowEvent.StartedTryConnect);      break;
				case ConnectionEvent.StartedTryDisconnect:   ApplyWorkflowEvent(WorkflowEvent.StartedTryDisconnect);   break;
			}
		}		

		public void TryConnect (Address serverAddress, Address clientAddress, Action<string> errorCallback)
		{
			connectionService.TryConnect(serverAddress, clientAddress, errorCallback);
		}

		public void TryDebugConnect(Address serverAddress, Address clientAddress, Action<string> errorCallback)
		{
			connectionService.TryDebugConnect(serverAddress, clientAddress, errorCallback);
		}

		public void TryDisconnect(Action<string> errorCallback)
		{
			connectionService.TryDisconnect(errorCallback);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                                    undo / redo                                    ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////


		public event Action<bool> UndoPossibleChanged;
		public event Action<bool> RedoPossibleChanged;

		public bool UndoPossible ()
		{
			return false;
		}

		public void Undo ()
		{			
		}

		public string GetUndoActionMessage ()
		{
			return "";
		}

		public bool RedoPossible ()
		{
			return false;
		}

		public void Redo ()
		{			
		}

		public string GetRedoActionMessage ()
		{
			return "";
		}
		
	}
}

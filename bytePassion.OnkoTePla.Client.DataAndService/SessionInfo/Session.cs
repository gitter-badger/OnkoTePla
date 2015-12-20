using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Contracts.Config;


namespace bytePassion.OnkoTePla.Client.DataAndService.SessionInfo
{
	public class Session : ISession
	{
	    private readonly IConnectionService connectionService;
	    private readonly IClientWorkflow clientWorkflow;
	    private ApplicationState currentApplicationState;
	    

	    internal Session(IConnectionService connectionService, IClientWorkflow clientWorkflow)
	    {
		    this.connectionService = connectionService;
		    this.clientWorkflow = clientWorkflow;

			CurrentApplicationState = clientWorkflow.CurrentState;
			AvailableUsers = new List<User>();

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
				currentApplicationState = value;
				ApplicationStateChanged?.Invoke(CurrentApplicationState);
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

		public User LoggedInUser { get; set; }

		public IReadOnlyList<User> AvailableUsers { get; private set; }


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

		public void TryConnect (Address serverAddress)
		{
			connectionService.TryConnect(serverAddress);
		}

		public void TryDisconnect ()
		{
			connectionService.TryDisconnect();
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

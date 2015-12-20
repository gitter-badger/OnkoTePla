using System;
using System.Collections.Generic;
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
	    private ApplicationState currentApplicationState;

		private IReadOnlyList<User> availableUsers;

	    private static Logger logger = LogManager.GetLogger("Session");

		internal Session(IConnectionService connectionService, IClientWorkflow clientWorkflow)
	    {
		    this.connectionService = connectionService;
		    this.clientWorkflow = clientWorkflow;

			CurrentApplicationState = clientWorkflow.CurrentState;

			AvailableUsers = new List<User>
			{
				new User("exampleUser1", new List<Guid> {Guid.Parse("9b95563a-039d-44b3-b95f-8ee7fabc41e3")}, "1234", Guid.Parse("f74605e6-3f54-4f08-b127-f52201d03d20")),
				new User("exampleUser2", new List<Guid> {Guid.Parse("9b95563a-039d-44b3-b95f-8ee7fabc41e3"),
														 Guid.Parse("d6c3e8c6-6281-4041-97ea-724a3d5379a5")}, "2345", Guid.Parse("1ca9e57c-9fee-42d9-8067-292abbfb29fb")),
			};

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
                logger.Debug("CurrentApplicationState change to: {0}", value);
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

		public event Action<IReadOnlyList<User>> UserListAvailable;

		public User LoggedInUser { get; set; }

		public IReadOnlyList<User> AvailableUsers
		{
			get { return availableUsers; }
			private set
			{
				availableUsers = value;
				UserListAvailable?.Invoke(AvailableUsers);
			}
		}

		public void TryLogin(User user, string password)
		{
			LoggedInUser = user;
			ApplyWorkflowEvent(WorkflowEvent.LoggedIn);
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

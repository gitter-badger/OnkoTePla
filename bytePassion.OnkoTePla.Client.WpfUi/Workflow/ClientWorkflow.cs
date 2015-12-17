using bytePassion.Lib.Utils.Workflows;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.WpfUi.Workflow
{
    internal class ClientWorkflow : StateEngine<ApplicationState, WorkflowCommands>, 
                                    IClientWorkflow
    {
        private static readonly IReadOnlyList<StateTransition<ApplicationState, WorkflowCommands>> Transitions = new List<StateTransition<ApplicationState, WorkflowCommands>>
        {
            new StateTransition<ApplicationState, WorkflowCommands>(ApplicationState.DisconnectedFromServer,  WorkflowCommands.Connect,    ApplicationState.ConnectedButNotLoggedIn),
            new StateTransition<ApplicationState, WorkflowCommands>(ApplicationState.ConnectedButNotLoggedIn, WorkflowCommands.LogIn,      ApplicationState.LoggedIn),
            new StateTransition<ApplicationState, WorkflowCommands>(ApplicationState.LoggedIn,                WorkflowCommands.LogOut,     ApplicationState.ConnectedButNotLoggedIn),
            new StateTransition<ApplicationState, WorkflowCommands>(ApplicationState.ConnectedButNotLoggedIn, WorkflowCommands.Disconnect, ApplicationState.DisconnectedFromServer),
            new StateTransition<ApplicationState, WorkflowCommands>(ApplicationState.LoggedIn,                WorkflowCommands.Disconnect, ApplicationState.DisconnectedFromServer)
        };
        
        public ClientWorkflow()
            : base(Transitions, ApplicationState.DisconnectedFromServer)
        {            
        }
    }
}

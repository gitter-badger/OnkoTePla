using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.Workflow
{
    internal interface IClientWorkflow
    {
        event Action<ApplicationState> StateChanged;

        ApplicationState CurrentState { get; }

        bool IsCommandApplyable(WorkflowCommands command);
        ApplicationState MoveNext(WorkflowCommands command);

    }
}
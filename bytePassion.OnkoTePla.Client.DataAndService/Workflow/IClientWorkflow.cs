using System;


namespace bytePassion.OnkoTePla.Client.DataAndService.Workflow
{
    public interface IClientWorkflow
    {
        event Action<ApplicationState> StateChanged;

        ApplicationState CurrentState { get; }

        bool IsCommandApplyable(WorkflowCommands command);
        ApplicationState MoveNext(WorkflowCommands command);

    }
}
namespace bytePassion.OnkoTePla.Client.DataAndService.Workflow
{
	public enum WorkflowEvent
    {
		StartedTryConnect,
		StartedTryDisconnect,
		ConnectionEstablished,
		ConAttemptUnsuccessful,		
		Disconnected,
		LoggedIn,
        LoggedOut,
        ConnectionLost		
    }
}
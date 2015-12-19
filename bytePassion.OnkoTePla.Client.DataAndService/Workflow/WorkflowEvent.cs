namespace bytePassion.OnkoTePla.Client.DataAndService.Workflow
{
	public enum WorkflowEvent
    {
		TryConnect,
		ConnectionEstablished,
		ConAttemptUnsuccessful,
		TryDisconnect,
		Disconnected,
		LoggedIn,
        LoggedOut,
        ConnectionLost		
    }
}
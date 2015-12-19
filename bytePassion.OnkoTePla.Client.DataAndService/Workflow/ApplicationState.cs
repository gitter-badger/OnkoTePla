namespace bytePassion.OnkoTePla.Client.DataAndService.Workflow
{
	public enum ApplicationState
    {
        DisconnectedFromServer,
		TryingToConnect,
		TryingToDisconnect,
        ConnectedButNotLoggedIn,
        LoggedIn
    }
}
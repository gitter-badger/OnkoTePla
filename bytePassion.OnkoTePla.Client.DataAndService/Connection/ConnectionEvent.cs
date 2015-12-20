namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal enum ConnectionEvent
	{
		StartedTryConnect,
		ConAttemptUnsuccessful,
		ConnectionEstablished,
		StartedTryDisconnect,
		Disconnected,
		ConnectionLost		
	}
}
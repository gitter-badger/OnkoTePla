namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage.Helper
{
	internal class ConnectedClientDisplayData
	{
		public ConnectedClientDisplayData(string sessionId, string connectionTime, string clientAddress)
		{
			SessionId = sessionId;
			ConnectionTime = connectionTime;
			ClientAddress = clientAddress;
		}

		public string SessionId      { get; }
		public string ConnectionTime { get; }
		public string ClientAddress  { get; }
	}
}
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage.Helper
{
	internal class ConnectedClientDisplayData : INotifyPropertyChanged
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
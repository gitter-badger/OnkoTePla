using System;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	public interface IConnectionService
	{
		event Action<ConnectionStatus> ConnectionStatusChanged;

		Address CurrentServerAddress { get; }
		ConnectionStatus ConnectionStatus { get; }
		
		void TryConnect (Address serverAddress);
		void TryDisconnect ();
	}
}
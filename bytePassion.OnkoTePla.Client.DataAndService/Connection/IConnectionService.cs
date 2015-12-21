using System;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal interface IConnectionService
	{
		event Action<ConnectionEvent> ConnectionEventInvoked;
		
		Address          ServerAddress    { get; }
		Address          ClientAddress    { get; }
		ConnectionStatus ConnectionStatus { get; }
		
		void TryConnect (Address serverAddress, Address clientAddress);
		void TryDisconnect ();
	}
}
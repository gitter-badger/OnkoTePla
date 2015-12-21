using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public interface IConnectionService : IDisposable
	{
		event Action<ConnectionSessionId> NewSessionStarted;
		event Action<ConnectionSessionId> SessionTerminated;
	}
}

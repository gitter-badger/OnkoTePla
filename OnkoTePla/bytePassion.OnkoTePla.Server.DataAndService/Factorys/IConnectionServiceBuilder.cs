using bytePassion.OnkoTePla.Server.DataAndService.Connection;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public interface IConnectionServiceBuilder
	{
		IConnectionService Build();
		void DisposeConnectionService(IConnectionService connectionService);
	}
}
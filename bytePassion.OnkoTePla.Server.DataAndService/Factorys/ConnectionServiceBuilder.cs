using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class ConnectionServiceBuilder : IConnectionServiceBuilder
	{

		private NetMQContext zmqContext;

		public IConnectionService Build()
		{
			
			zmqContext = NetMQContext.Create();

			return new ConnectionService(zmqContext);
		}

		public void DisposeConnectionService(IConnectionService connectionService)
		{
			connectionService.Dispose();
			zmqContext.Dispose();
		}
	}
}

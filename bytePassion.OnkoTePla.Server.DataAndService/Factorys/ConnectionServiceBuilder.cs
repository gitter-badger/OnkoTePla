using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class ConnectionServiceBuilder : IConnectionServiceBuilder
	{
		private readonly IDataCenter dataCenter;

		public ConnectionServiceBuilder(IDataCenter dataCenter)
		{
			this.dataCenter = dataCenter;
		}

		private NetMQContext zmqContext;

		public IConnectionService Build()
		{
			
			zmqContext = NetMQContext.Create();

			return new ConnectionService(zmqContext, dataCenter);
		}

		public void DisposeConnectionService(IConnectionService connectionService)
		{
			connectionService.Dispose();
			zmqContext.Dispose();
		}
	}
}

using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class ConnectionServiceBuilder : IConnectionServiceBuilder
	{
		private readonly IDataCenter dataCenter;
		private readonly IReadModelRepository readModelRepository;

		public ConnectionServiceBuilder(IDataCenter dataCenter, IReadModelRepository readModelRepository)
		{
			this.dataCenter = dataCenter;
			this.readModelRepository = readModelRepository;
		}

		private NetMQContext zmqContext;

		public IConnectionService Build()
		{
			
			zmqContext = NetMQContext.Create();
			//var sessionRepository = new CurrentSessionsInfo();

			return new ConnectionService(zmqContext, dataCenter, readModelRepository);
		}
		
		public void DisposeConnectionService(IConnectionService connectionService)
		{
			connectionService.Dispose();
			zmqContext.Dispose();
		}
	}
}

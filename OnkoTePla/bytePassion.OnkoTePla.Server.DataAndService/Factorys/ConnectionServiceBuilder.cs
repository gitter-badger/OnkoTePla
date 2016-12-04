using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class ConnectionServiceBuilder : IConnectionServiceBuilder
	{
		private readonly DataCenterContainer dataCenterContainer;				

		public ConnectionServiceBuilder(DataCenterContainer dataCenterContainer)
		{
			this.dataCenterContainer = dataCenterContainer;			
		}

		private NetMQContext zmqContext;

		public IConnectionService Build()
		{			
			zmqContext = NetMQContext.Create();
			
			return new ConnectionService(zmqContext, dataCenterContainer); 
		}
		
		public void DisposeConnectionService(IConnectionService connectionService)
		{
			connectionService.Dispose();
			zmqContext.Dispose();
		}
	}
}

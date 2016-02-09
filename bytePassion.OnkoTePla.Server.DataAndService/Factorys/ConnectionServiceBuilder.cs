using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Factorys
{
	public class ConnectionServiceBuilder : IConnectionServiceBuilder
	{
		private readonly IDataCenter dataCenter;
		private readonly IEventStore eventStore;


		public ConnectionServiceBuilder(IDataCenter dataCenter, IEventStore eventStore)
		{
			this.dataCenter = dataCenter;
			this.eventStore = eventStore;			
		}

		private NetMQContext zmqContext;

		public IConnectionService Build()
		{			
			zmqContext = NetMQContext.Create();
			
			return new ConnectionService(zmqContext, dataCenter, eventStore); 
		}
		
		public void DisposeConnectionService(IConnectionService connectionService)
		{
			connectionService.Dispose();
			zmqContext.Dispose();
		}
	}
}

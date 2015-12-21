using bytePassion.OnkoTePla.Communication.Connection;
using NetMQ;

namespace bytePassion.OnkoTePla.Communication.Factorys
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
			zmqContext.Dispose();
		}
	}

	public interface IConnectionServiceBuilder
	{
		
	}
}

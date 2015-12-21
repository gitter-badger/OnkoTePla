using NetMQ;

namespace bytePassion.OnkoTePla.Communication.Connection
{
	public class ConnectionService : IConnectionService
	{
		private readonly NetMQContext zmqContext;

		internal ConnectionService(NetMQContext zmqContext)
		{
			this.zmqContext = zmqContext;			
		}
	}
}
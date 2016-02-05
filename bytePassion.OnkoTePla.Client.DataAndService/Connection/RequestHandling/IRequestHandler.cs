using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling
{
	internal interface IRequestHandler
	{
		void HandleRequest(RequestSocket socket);
	}
}

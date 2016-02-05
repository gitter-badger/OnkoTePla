using bytePassion.OnkoTePla.Communication.NetworkMessages;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling
{
	internal interface IResponseHandlerFactory
	{
		IResponseHandler<TRequest> GetHandler<TRequest>(TRequest request, ResponseSocket socket) 
			where TRequest : NetworkMessageBase;
	}
}
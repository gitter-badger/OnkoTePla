using bytePassion.OnkoTePla.Communication.NetworkMessages;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling
{
	internal interface IResponseHandler<in TRequest> where TRequest : NetworkMessageBase
	{
		void Handle(TRequest request);
	}
}
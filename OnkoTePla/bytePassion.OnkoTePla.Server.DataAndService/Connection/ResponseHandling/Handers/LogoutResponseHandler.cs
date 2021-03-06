using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class LogoutResponseHandler : ResponseHandlerBase<LogoutRequest>
	{
		public LogoutResponseHandler(ICurrentSessionsInfo sessionRepository, 
									 ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
		}

		public override void Handle(LogoutRequest request)
		{			
			if (!IsRequestValid(request.SessionId, request.UserId))
				return;

			SessionRepository.UpdateLoggedInUser(request.SessionId, null);

			Socket.SendNetworkMsg(new LogoutResponse());
		}
	}
}
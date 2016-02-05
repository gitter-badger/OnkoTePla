using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetAccessablePracticesRequestHandler : ResponseHandlerBase<GetAccessablePracticesRequest>
	{
		public GetAccessablePracticesRequestHandler(ICurrentSessionsInfo sessionRepository, 
													ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
		}

		public override void Handle(GetAccessablePracticesRequest request)
		{
			if (!ValidateRequest(request.SessionId, request.UserId))
				return;

			Socket.SendNetworkMsg(new GetAccessablePracticesResponse(
				SessionRepository.GetSessionInfo(request.SessionId).LoggedInUser.ListOfAccessableMedicalPractices)
			);
		}
	}
}
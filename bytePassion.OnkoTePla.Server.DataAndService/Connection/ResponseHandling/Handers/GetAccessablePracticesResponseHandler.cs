using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetAccessablePracticesResponseHandler : ResponseHandlerBase<GetAccessablePracticesRequest>
	{
		public GetAccessablePracticesResponseHandler(ICurrentSessionsInfo sessionRepository, 
													 ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
		}

		public override void Handle(GetAccessablePracticesRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId))
				return;

			Socket.SendNetworkMsg(new GetAccessablePracticesResponse(
				SessionRepository.GetSessionInfo(request.SessionId).LoggedInUser.ListOfAccessableMedicalPractices)
			);
		}
	}
}
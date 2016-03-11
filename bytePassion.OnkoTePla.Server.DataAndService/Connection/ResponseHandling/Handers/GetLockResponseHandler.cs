using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetLockResponseHandler : ResponseHandlerBase<GetLockRequest>
	{
		private readonly ICurrentSessionsInfo sessionRepository;

		public GetLockResponseHandler(ICurrentSessionsInfo sessionRepository, 
									  ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
			this.sessionRepository = sessionRepository;
		}

		public override void Handle(GetLockRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			var lockGranted = sessionRepository.TryToGetLock(request.MedicalPracticeId, request.Day);
				
			Socket.SendNetworkMsg(new GetLockResponse(lockGranted));									
		}
	}
}
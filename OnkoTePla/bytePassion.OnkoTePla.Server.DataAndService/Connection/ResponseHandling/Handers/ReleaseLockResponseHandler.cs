using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class ReleaseLockResponseHandler : ResponseHandlerBase<ReleaseLockRequest>
	{
		private readonly ICurrentSessionsInfo sessionRepository;

		public ReleaseLockResponseHandler(ICurrentSessionsInfo sessionRepository, 
			ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
			this.sessionRepository = sessionRepository;
		}

		public override void Handle(ReleaseLockRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			sessionRepository.ReleaseLock(request.MedicalPracticeId, request.Day);

			Socket.SendNetworkMsg(new ReleaseLockResponse());
		}
	}
}
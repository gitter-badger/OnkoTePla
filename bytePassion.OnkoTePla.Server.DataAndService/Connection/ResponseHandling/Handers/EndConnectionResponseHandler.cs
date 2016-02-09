using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class EndConnectionResponseHandler : ResponseHandlerBase<EndConnectionRequest>
	{
		private readonly IHeartbeatThreadCollection heartbeatThreadCollection;

		public EndConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
										    ResponseSocket socket,
										    IHeartbeatThreadCollection heartbeatThreadCollection) 
			: base(sessionRepository, socket)
		{
			this.heartbeatThreadCollection = heartbeatThreadCollection;
		}

		public override void Handle(EndConnectionRequest request)
		{
			if (!IsRequestValid(request.SessionId))
				return;

			SessionRepository.RemoveSession(request.SessionId);
			heartbeatThreadCollection.StopThread(request.SessionId);

			Socket.SendNetworkMsg(new EndConnectionResponse());
		}
	}
}
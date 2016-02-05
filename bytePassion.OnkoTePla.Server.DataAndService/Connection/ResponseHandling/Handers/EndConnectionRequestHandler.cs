using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class EndConnectionRequestHandler : ResponseHandlerBase<EndConnectionRequest>
	{
		private readonly HeartbeatThreadCollection heartbeatThreadCollection;

		public EndConnectionRequestHandler(ICurrentSessionsInfo sessionRepository, 
										   ResponseSocket socket,
										   HeartbeatThreadCollection heartbeatThreadCollection) 
			: base(sessionRepository, socket)
		{
			this.heartbeatThreadCollection = heartbeatThreadCollection;
		}

		public override void Handle(EndConnectionRequest request)
		{
			if (!ValidateRequest(request.SessionId))
				return;

			SessionRepository.RemoveSession(request.SessionId);
			heartbeatThreadCollection.StopThread(request.SessionId);

			Socket.SendNetworkMsg(new EndConnectionResponse());
		}
	}
}
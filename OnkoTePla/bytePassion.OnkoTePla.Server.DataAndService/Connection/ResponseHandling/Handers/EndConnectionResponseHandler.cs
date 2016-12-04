using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class EndConnectionResponseHandler : ResponseHandlerBase<EndConnectionRequest>
	{
		private readonly Action<ConnectionSessionId> connectionEndedCallback;		

		public EndConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
										    ResponseSocket socket,										    
											Action<ConnectionSessionId> connectionEndedCallback) 
			: base(sessionRepository, socket)
		{
			this.connectionEndedCallback = connectionEndedCallback;			
		}

		public override void Handle(EndConnectionRequest request)
		{
			if (!IsRequestValid(request.SessionId))
				return;

			SessionRepository.RemoveSession(request.SessionId);

			connectionEndedCallback(request.SessionId);		

			Socket.SendNetworkMsg(new EndConnectionResponse());
		}
	}
}
using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class BeginDebugConnectionResponseHandler : ResponseHandlerBase<BeginDebugConnectionRequest>
	{
		public BeginDebugConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
												   ResponseSocket socket) 
			: base(sessionRepository, socket)
		{
		}

		public override void Handle(BeginDebugConnectionRequest request)
		{
			if (SessionRepository.IsClientAddressConnected(request.ClientAddress))
			{
				const string errorMsg = "a connection from that address is already established";
				Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}

			var newSessionId = new ConnectionSessionId(Guid.NewGuid());
			SessionRepository.AddSession(newSessionId, TimeTools.GetCurrentTimeStamp().Item2, request.ClientAddress, true);

			Socket.SendNetworkMsg(new BeginDebugConnectionResponse(newSessionId));
		}
	}
}
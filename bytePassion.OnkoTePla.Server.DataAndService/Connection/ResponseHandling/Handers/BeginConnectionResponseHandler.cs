using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class BeginConnectionResponseHandler : ResponseHandlerBase<BeginConnectionRequest>
	{
		private readonly IHeartbeatThreadCollection heartbeatThreadCollection;
		
		public BeginConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
											  ResponseSocket socket,
											  IHeartbeatThreadCollection heartbeatThreadCollection) 
			: base(sessionRepository, socket)
		{
			this.heartbeatThreadCollection = heartbeatThreadCollection;
		}

		public override void Handle(BeginConnectionRequest request)
		{
			if (SessionRepository.IsClientAddressConnected(request.ClientAddress))
			{
				const string errorMsg = "a connection from that address is already established";
				Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}

			var newSessionId = new ConnectionSessionId(Guid.NewGuid());
			SessionRepository.AddSession(newSessionId, TimeTools.GetCurrentTimeStamp().Item2, request.ClientAddress, false);
			
			heartbeatThreadCollection.AddThread(request.ClientAddress, newSessionId);

			Socket.SendNetworkMsg(new BeginConnectionResponse(newSessionId));
		}
	}
}
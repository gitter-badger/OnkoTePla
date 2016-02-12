using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class BeginDebugConnectionResponseHandler : ResponseHandlerBase<BeginDebugConnectionRequest>
	{
		private readonly Action<AddressIdentifier, ConnectionSessionId> newDebugConnectionEstablishedCallback;

		public BeginDebugConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
												   ResponseSocket socket, 
												   Action<AddressIdentifier, ConnectionSessionId> newDebugConnectionEstablishedCallback) 
			: base(sessionRepository, socket)
		{
			this.newDebugConnectionEstablishedCallback = newDebugConnectionEstablishedCallback;
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

			newDebugConnectionEstablishedCallback(request.ClientAddress, newSessionId);

			Socket.SendNetworkMsg(new BeginDebugConnectionResponse(newSessionId));
		}
	}
}
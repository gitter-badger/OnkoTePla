using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class BeginConnectionResponseHandler : ResponseHandlerBase<BeginConnectionRequest>
	{
		private readonly Action<AddressIdentifier, ConnectionSessionId> newConnectionEstablishedCallback;		

		public BeginConnectionResponseHandler(ICurrentSessionsInfo sessionRepository, 
											  ResponseSocket socket,
											  Action<AddressIdentifier, ConnectionSessionId> newConnectionEstablishedCallback) 
			: base(sessionRepository, socket)
		{
			this.newConnectionEstablishedCallback = newConnectionEstablishedCallback;			
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
						
			newConnectionEstablishedCallback(request.ClientAddress, newSessionId);			

			Socket.SendNetworkMsg(new BeginConnectionResponse(newSessionId));
		}
	}
}
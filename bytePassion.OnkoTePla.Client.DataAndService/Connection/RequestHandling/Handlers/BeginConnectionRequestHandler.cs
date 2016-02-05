using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class BeginConnectionRequestHandler : RequestHandlerBase
	{
		private readonly AddressIdentifier clientAddress;
		private readonly Action<ConnectionSessionId> connectionSuccessfulCallback;


		public BeginConnectionRequestHandler (Action<ConnectionSessionId> connectionSuccessfulCallback,
											  AddressIdentifier clientAddress, 											  
								              Action<string> errorCallback)  			
			: base(errorCallback)
		{
			this.clientAddress = clientAddress;
			this.connectionSuccessfulCallback = connectionSuccessfulCallback;
		}
				

		public override void HandleRequest(RequestSocket socket)
		{			
			HandleRequestHelper<BeginConnectionRequest, BeginConnectionResponse>(
				new BeginConnectionRequest(clientAddress),
				socket,				
				beginConnectionResponse => connectionSuccessfulCallback(beginConnectionResponse.SessionId)
			);			
		}
	}
}
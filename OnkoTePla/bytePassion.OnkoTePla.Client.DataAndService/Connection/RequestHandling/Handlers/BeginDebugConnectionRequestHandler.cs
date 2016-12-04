using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class BeginDebugConnectionRequestHandler : RequestHandlerBase
	{
		private readonly AddressIdentifier clientAddress;
		private readonly Action<ConnectionSessionId> connectionSuccessfulCallback;


		public BeginDebugConnectionRequestHandler (Action<ConnectionSessionId> connectionSuccessfulCallback,
												   AddressIdentifier clientAddress,
												   Action<string> errorCallback)
			: base(errorCallback)
		{
			this.clientAddress = clientAddress;
			this.connectionSuccessfulCallback = connectionSuccessfulCallback;
		}
		

		public override void HandleRequest(RequestSocket socket)
		{			
			HandleRequestHelper<BeginDebugConnectionRequest, BeginDebugConnectionResponse>(
				new BeginDebugConnectionRequest(clientAddress),
				socket,				
				beginDebugConnectionResponse => connectionSuccessfulCallback(beginDebugConnectionResponse.SessionId)
			);			
		}
	}
}
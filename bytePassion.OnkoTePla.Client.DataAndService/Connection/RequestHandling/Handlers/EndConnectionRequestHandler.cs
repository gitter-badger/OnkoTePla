using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class EndConnectionRequestHandler : RequestHandlerBase
	{
		private readonly Action connectionEndedCallback;
		private readonly ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable; 


		public EndConnectionRequestHandler (Action connectionEndedCallback,
											ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable,
											Action<string> errorCallback)
			: base(errorCallback)
		{
			this.connectionEndedCallback = connectionEndedCallback;
			this.sessionIdVariable = sessionIdVariable;
		}

		
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<EndConnectionRequest, EndConnectionResponse>(
				new EndConnectionRequest(sessionIdVariable.Value),
				socket,				
				endConnectionResponse => connectionEndedCallback()
			);
		}
	}
}
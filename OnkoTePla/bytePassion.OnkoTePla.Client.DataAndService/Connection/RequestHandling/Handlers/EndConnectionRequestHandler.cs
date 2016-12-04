using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class EndConnectionRequestHandler : RequestHandlerBase
	{
		private readonly Action connectionEndedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable; 


		public EndConnectionRequestHandler (Action connectionEndedCallback,
											ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
											Action<string> errorCallback)
			: base(errorCallback)
		{
			this.connectionEndedCallback = connectionEndedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
		}

		
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<EndConnectionRequest, EndConnectionResponse>(
				new EndConnectionRequest(connectionInfoVariable.Value.SessionId),
				socket,				
				endConnectionResponse => connectionEndedCallback()
			);
		}
	}
}
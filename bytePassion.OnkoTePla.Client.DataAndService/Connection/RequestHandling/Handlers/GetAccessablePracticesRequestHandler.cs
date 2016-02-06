using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetAccessablePracticesRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<Guid>> dataReceivedCallback;		
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;

		public GetAccessablePracticesRequestHandler(Action<IReadOnlyList<Guid>> dataReceivedCallback, 													
												    ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable, 
												    Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;			
			this.connectionInfoVariable = connectionInfoVariable;
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetAccessablePracticesRequest, GetAccessablePracticesResponse>(
				new GetAccessablePracticesRequest(connectionInfoVariable.Value.SessionId, 
												  connectionInfoVariable.Value.LoggedInUser.Id),
				socket,
				response => dataReceivedCallback(response.AccessableMedicalPractices)
			);
		}
	}
}

using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetTherapyPlaceListRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<TherapyPlaceType>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;

		public GetTherapyPlaceListRequestHandler(Action<IReadOnlyList<TherapyPlaceType>> dataReceivedCallback, 
												 ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable, 
												 Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
		}
		
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetTherapyPlacesTypeListRequest, GetTherapyPlacesTypeListResponse>(
				new GetTherapyPlacesTypeListRequest(connectionInfoVariable.Value.SessionId, connectionInfoVariable.Value.LoggedInUser.Id),
				socket,
				response => dataReceivedCallback(response.TherapyPlaceTypes) 	
			);
		}
	}
}
using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetUserListRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<ClientUserData>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;


		public GetUserListRequestHandler (Action<IReadOnlyList<ClientUserData>> dataReceivedCallback, 
									      ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
									      Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
		}


		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetUserListRequest, GetUserListResponse>(
				new GetUserListRequest(connectionInfoVariable.Value.SessionId),
				socket,				
				userListResponse => dataReceivedCallback(userListResponse.AvailableUsers)
			);
		}
	}
}

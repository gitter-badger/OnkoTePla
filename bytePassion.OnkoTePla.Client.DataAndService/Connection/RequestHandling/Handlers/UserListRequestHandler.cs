using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class UserListRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<ClientUserData>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable;


		public UserListRequestHandler (Action<IReadOnlyList<ClientUserData>> dataReceivedCallback, 
									   ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable,
									   Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.sessionIdVariable = sessionIdVariable;
		}


		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetUserListRequest, GetUserListResponse>(
				new GetUserListRequest(sessionIdVariable.Value),
				socket,				
				userListResponse => dataReceivedCallback(userListResponse.AvailableUsers)
			);
		}
	}
}

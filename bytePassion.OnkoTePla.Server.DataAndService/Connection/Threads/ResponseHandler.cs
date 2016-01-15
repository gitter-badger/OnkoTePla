using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.DataRequests;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal static class ResponseHandler
	{
		#region UserListRequest

		public static void HandleUserListRequest(UserListRequest request, IList<SessionInfo> connectedSessions,
												 ResponseSocket socket, IDataCenter dataCenter)
		{
			if (connectedSessions.All(session => session.SessionId != request.SessionId))
			{
				var errorResponse = NetworkMessageCoding.Decode(new ErrorResponse("the session-ID is invalid"));
				socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
				return;
			}

			var userList = dataCenter.GetAllUsers()
									 .Select(user => new ClientUserData(user.ToString(), user.Id))
									 .ToList();

			var response = NetworkMessageCoding.Decode(new UserListResponse(userList));
			socket.SendAString(response, TimeSpan.FromSeconds(2));
		}

		#endregion
	}
}

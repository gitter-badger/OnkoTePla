using System;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal static class RequestHandler
	{
		#region userListRequest
		 
		public static void HandleUserListRequest(UserListRequestObject userListRequest, 
												 ConnectionSessionId sessionId, RequestSocket socket)
		{
		 	
			var outMessage = NetworkMessageCoding.Encode(new UserListRequest(sessionId));
			socket.SendAString(outMessage, TimeSpan.FromSeconds(2));

			var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(5));
			var response = NetworkMessageCoding.Decode(inMessage);

			switch (response.Type)
			{
				case NetworkMessageType.GetUserListResponse:
				{
					var userListResponse = (UserListResponse) response;
					userListRequest.DataReceivedCallback(userListResponse.AvailableUsers);
					break;
				}
				case NetworkMessageType.ErrorResponse:
				{
					var errorResponse = (ErrorResponse) response;
					userListRequest.ErrorCallback(errorResponse.ErrorMessage);
					break;
				}
				default:
				{
					throw new ArgumentException();					
				}
			}
		}

		#endregion
	}
}

using System;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal static class RequestHandler
	{
		
		private static void HandleRequest<TRequest, TResponse>(TRequest request, NetMQSocket socket, 
										  Action<string> errorCallback,
										  Action<TResponse> responseReceived)

			where TRequest : NetworkMessageBase
			where TResponse : NetworkMessageBase
		{
			var outMessage = NetworkMessageCoding.Encode(request);
			var sendingSuccessful = socket.SendAString(outMessage, TimeSpan.FromSeconds(2));

			if (!sendingSuccessful)
				errorCallback("failed sending");

			var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(5));

			if (inMessage == "")
				errorCallback("no response received");

			var response = NetworkMessageCoding.Decode(inMessage);

			var responseMsg = response as TResponse;
			if (responseMsg != null)
			{
				responseReceived(responseMsg);
				return;
			}

			if (response.Type == NetworkMessageType.ErrorResponse)
			{
				var errorResponse = (ErrorResponse) response;
				errorCallback(errorResponse.ErrorMessage);
				return;
			}

			throw new ArgumentException($"unexpected Message-Type: {response.Type}");			
		}
		
		 
		public static void HandleUserListRequest(UserListRequestObject userListRequest, 
												 ConnectionSessionId sessionId, RequestSocket socket)
		{		 	
			HandleRequest<UserListRequest, UserListResponse>(
				new UserListRequest(sessionId), 
				socket, 
				userListRequest.ErrorCallback,
				userListResponse => userListRequest.DataReceivedCallback(userListResponse.AvailableUsers)
			);
		}
		
		public static void HandleLoginRequest(LoginRequestObject loginRequest,
											  ConnectionSessionId sessionId, RequestSocket socket)
		{
			HandleRequest<LoginRequest, LoginResponse>(
				new LoginRequest(sessionId, loginRequest.User.Id, loginRequest.Password),
				socket,
				loginRequest.ErrorCallback,
				loginResponse => loginRequest.LoginSuccessfulCallback()
			);			
		}
	}
}

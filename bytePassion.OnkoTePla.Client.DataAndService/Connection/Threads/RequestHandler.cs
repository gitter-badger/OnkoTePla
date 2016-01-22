using System;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
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

			where TRequest  : NetworkMessageBase
			where TResponse : NetworkMessageBase
		{			
			var sendingSuccessful = socket.SendNetworkMsg(request);

			if (!sendingSuccessful)
			{
				errorCallback("failed sending");
				return;
			}

			var response = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(5));

			if (response == null)
			{
				errorCallback("no response received");
				return;
			}

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

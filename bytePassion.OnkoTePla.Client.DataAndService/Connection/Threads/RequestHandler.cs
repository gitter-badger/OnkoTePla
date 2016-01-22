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

		public static void HandleLogoutRequest(LogoutRequestObject logoutRequest,
											   ConnectionSessionId sessionId, RequestSocket socket)
		{
			HandleRequest<LogoutRequest, LogoutResponse>(
				new LogoutRequest(sessionId, logoutRequest.User.Id),
				socket,
				logoutRequest.ErrorCallback,
				logoutResponse => logoutRequest.LogoutSuccessfulCallback() 
			);
		}

		public static void HandleBeginConnectionRequest(BeginConnectionRequestObject beginConnectionRequest,
														out ConnectionSessionId sessionId, RequestSocket socket)
		{
			ConnectionSessionId newSessionId = null;

			HandleRequest<BeginConnectionRequest, BeginConnectionResponse>(
				new BeginConnectionRequest(beginConnectionRequest.ClientAddress),
				socket,
				beginConnectionRequest.ErrorCallback,
				beginConnectionResponse =>
				{
					newSessionId = beginConnectionResponse.SessionId;
					beginConnectionRequest.ConnectionSuccessfulCallback(beginConnectionResponse.SessionId);
				} 	
			);

			sessionId = newSessionId;
		}

		public static void HandleBeginDebugConnectionRequest (BeginDebugConnectionRequestObject beginDebugConnectionRequest,
														      out ConnectionSessionId sessionId, RequestSocket socket)
		{
			ConnectionSessionId newSessionId = null;

			HandleRequest<BeginDebugConnectionRequest, BeginDebugConnectionResponse>(
				new BeginDebugConnectionRequest(beginDebugConnectionRequest.ClientAddress),
				socket,
				beginDebugConnectionRequest.ErrorCallback,
				beginDebugConnectionResponse =>
				{
					newSessionId = beginDebugConnectionResponse.SessionId;
					beginDebugConnectionRequest.ConnectionSuccessfulCallback(beginDebugConnectionResponse.SessionId);
				}
			);

			sessionId = newSessionId;
		}
		 
		public static void HandleEndConnectionRequest (EndConnectionRequestObject endConnectionRequest,
													   ConnectionSessionId sessionId, RequestSocket socket)
		{
			HandleRequest<EndConnectionRequest, EndConnectionResponse>(
				new EndConnectionRequest(sessionId),
				socket,
				endConnectionRequest.ErrorCallback,
				endConnectionResponse => endConnectionRequest.ConnectionEndedCallback()
			);
		}
	}
}

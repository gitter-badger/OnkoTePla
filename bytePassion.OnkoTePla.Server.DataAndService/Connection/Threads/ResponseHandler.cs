using System;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal static class ResponseHandler
	{
		private static bool ValidateRequest (ConnectionSessionId sessionId,
											 ICurrentSessionsInfo sessionRepository,
											 NetMQSocket socket,
											 Guid? userId = null,
											 Guid? accessedMedPracticeId = null)
		{
			if (!sessionRepository.DoesSessionExist(sessionId))
			{
				const string errorMsg = "the session-ID is invalid";
				socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return false;
			}
						
			if (userId.HasValue)
			{
				var sessionInfo = sessionRepository.GetSessionInfo(sessionId);

				if (sessionInfo.LoggedInUser.Id != userId.Value)
				{					
					socket.SendNetworkMsg(new ErrorResponse("the user is not logged in"));
					return false;
				}

				if (accessedMedPracticeId.HasValue)
					if (!sessionInfo.LoggedInUser.ListOfAccessableMedicalPractices.Contains(accessedMedPracticeId.Value))
					{
						const string errorMsg = "the user has not the right to access this medical Practice";
						socket.SendNetworkMsg(new ErrorResponse(errorMsg));
						return false;
					}
			}

			return true;
		}

		#region UserListRequest

		public static void HandleUserListRequest(UserListRequest request, ICurrentSessionsInfo sessionRepository,
												 ResponseSocket socket, IDataCenter dataCenter)
		{
			var isRequestValid = ValidateRequest(request.SessionId, sessionRepository, socket);

			if (!isRequestValid)
				return;

			var userList = dataCenter.GetAllUsers()
									 .Where(user => !user.IsHidden)
									 .Select(user => new ClientUserData(user.ToString(), user.Id))
									 .ToList();
			
			socket.SendNetworkMsg(new UserListResponse(userList));
		}

		#endregion

		#region LoginRequest

		public static void HandleLoginRequest(LoginRequest request, ICurrentSessionsInfo sessionRepository,
											  ResponseSocket socket, IDataCenter dataCenter)
		{
			var isRequestValid = ValidateRequest(request.SessionId, sessionRepository, socket);

			if (!isRequestValid)
				return;

			if (sessionRepository.IsUserLoggedIn(request.UserId))
			{
				var sessionInfo = sessionRepository.GetSessionForUser(request.UserId);

				if (sessionInfo != null && sessionInfo.SessionId != request.SessionId)
				{
					const string errorMsg = "the user is already logged in";
					socket.SendNetworkMsg(new ErrorResponse(errorMsg));
					return;
				}
			}

			var user = dataCenter.GetUser(request.UserId);

			if (user.Password != request.Password)
			{
				const string errorMsg = "the password is incorrect";
				socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}
			
			socket.SendNetworkMsg(new LoginResponse());			

			sessionRepository.UpdateLoggedInUser(request.SessionId, user);
		}

		#endregion
		
		#region LogoutRequest
		
		public static void HandleLogoutRequest(LogoutRequest request, ICurrentSessionsInfo sessionRepository,
											   ResponseSocket socket)
		{
			var requestIsValid = ValidateRequest(request.SessionId, sessionRepository, socket, request.UserId);

			if (!requestIsValid)
				return;

			// todo handle logout
		}

		#endregion

		#region BeginConnectionRequest

		public static void HandleBeginConnectionRequest(BeginConnectionRequest request, 
														ICurrentSessionsInfo sessionRepository,
														ResponseSocket socket, 
														Action<AddressIdentifier, ConnectionSessionId> newConnectionCallback)
		{
			if (sessionRepository.IsClientAddressConnected(request.ClientAddress))
			{
				const string errorMsg = "a connection from that address is already established";
				socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}

			var newSessionId = new ConnectionSessionId(Guid.NewGuid());
			sessionRepository.AddSession(newSessionId, TimeTools.GetCurrentTimeStamp().Item2, request.ClientAddress, false);
			newConnectionCallback(request.ClientAddress, newSessionId);

			socket.SendNetworkMsg(new BeginConnectionResponse(newSessionId));
		}

		#endregion

		#region BeginDebugConnectionRequest

		public static void HandleBeginDebugConnectionRequest (BeginDebugConnectionRequest request, ICurrentSessionsInfo sessionRepository,
															  ResponseSocket socket)
		{
			if (sessionRepository.IsClientAddressConnected(request.ClientAddress))
			{
				const string errorMsg = "a connection from that address is already established";
				socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}

			var newSessionId = new ConnectionSessionId(Guid.NewGuid());
			sessionRepository.AddSession(newSessionId, TimeTools.GetCurrentTimeStamp().Item2, request.ClientAddress, true);			
			
			socket.SendNetworkMsg(new BeginDebugConnectionResponse(newSessionId));
		}

		#endregion

		#region EndConnectionRequest

		public static void HandleEndConnectionRequest(EndConnectionRequest request, ICurrentSessionsInfo sessionRepository,
													  ResponseSocket socket)
		{
			var requestIsValid = ValidateRequest(request.SessionId, sessionRepository, socket);

			if (!requestIsValid)
				return;

			sessionRepository.RemoveSession(request.SessionId);

			socket.SendNetworkMsg(new EndConnectionResponse());
		}

		#endregion
	}
}

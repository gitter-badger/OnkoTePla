using System;
using System.Linq;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal static class ResponseHandler
	{
		#region UserListRequest

		public static void HandleUserListRequest(UserListRequest request, ICurrentSessionsInfo sessionRepository,
												 ResponseSocket socket, IDataCenter dataCenter)
		{
			if (!sessionRepository.DoesSessionExist(request.SessionId))
			{
				var errorResponse = NetworkMessageCoding.Encode(new ErrorResponse("the session-ID is invalid"));
				socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
				return;
			}

			var userList = dataCenter.GetAllUsers()
									 .Where(user => !user.IsHidden)
									 .Select(user => new ClientUserData(user.ToString(), user.Id))
									 .ToList();

			var response = NetworkMessageCoding.Encode(new UserListResponse(userList));
			socket.SendAString(response, TimeSpan.FromSeconds(2));
		}

		#endregion

		#region LoginRequest

		public static void HandleLoginRequest(LoginRequest request, ICurrentSessionsInfo sessionRepository,
											  ResponseSocket socket, IDataCenter dataCenter)
		{
			if (!sessionRepository.DoesSessionExist(request.SessionId))
			{
				var errorResponse = NetworkMessageCoding.Encode(new ErrorResponse("the session-ID is invalid"));
				socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
				return;
			}

			if (sessionRepository.IsUserLoggedIn(request.UserId))
			{
				var sessionInfo = sessionRepository.GetSessionForUser(request.UserId);

				if (sessionInfo != null && sessionInfo.SessionId != request.SessionId)
				{
					var errorResponse = NetworkMessageCoding.Encode(new ErrorResponse("the user is already logged in"));
					socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
					return;
				}
			}

			var user = dataCenter.GetUser(request.UserId);

			if (user.Password != request.Password)
			{
				var errorResponse = NetworkMessageCoding.Encode(new ErrorResponse("the password is incorrect"));
				socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
				return;
			}

			var response = NetworkMessageCoding.Encode(new LoginResponse());
			socket.SendAString(response, TimeSpan.FromSeconds(2));			

			sessionRepository.UpdateLoggedInUser(request.SessionId, user);
		}

		#endregion
	}
}

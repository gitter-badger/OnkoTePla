using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class LoginRequestHandler : ResponseHandlerBase<LoginRequest>
	{
		private readonly IDataCenter dataCenter;

		public LoginRequestHandler(ICurrentSessionsInfo sessionRepository, 
								   ResponseSocket socket, 
								   IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(LoginRequest request)
		{
			if (!ValidateRequest(request.SessionId))
				return;

			if (SessionRepository.IsUserLoggedIn(request.UserId))
			{
				var sessionInfo = SessionRepository.GetSessionForUser(request.UserId);

				if (sessionInfo != null && sessionInfo.SessionId != request.SessionId)
				{
					const string errorMsg = "the user is already logged in";
					Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
					return;
				}
			}

			var user = dataCenter.GetUser(request.UserId);

			if (user.Password != request.Password)
			{
				const string errorMsg = "the password is incorrect";
				Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return;
			}

			SessionRepository.UpdateLoggedInUser(request.SessionId, user);

			Socket.SendNetworkMsg(new LoginResponse());
		}
	}
}
using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class LoginRequestHandler : RequestHandlerBase
	{
		private readonly ISharedState<ConnectionInfo> connectionInfoVariable;
		private readonly ClientUserData user;
		private readonly string password;
		private readonly Action loginSuccessfulCallback;


		public LoginRequestHandler (Action loginSuccessfulCallback, 
								    ClientUserData user, string password, 
									ISharedState<ConnectionInfo> connectionInfoVariable,
									Action<string> errorCallback) 
			: base(errorCallback)
			
		{
			this.connectionInfoVariable = connectionInfoVariable;
			this.user = user;
			this.password = password;
			this.loginSuccessfulCallback = loginSuccessfulCallback;
		}
		
				
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<LoginRequest, LoginResponse>(
				new LoginRequest(connectionInfoVariable.Value.SessionId, user.Id, password),
				socket,
				loginResponse =>
				{
					connectionInfoVariable.Value = new ConnectionInfo(connectionInfoVariable.Value.SessionId, user);
					loginSuccessfulCallback();
				}
			);
		}
	}
}
using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class LoginRequestHandler : RequestHandlerBase
	{
		private readonly ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable;
		private readonly ClientUserData user;
		private readonly string password;
		private readonly Action loginSuccessfulCallback;


		public LoginRequestHandler (Action loginSuccessfulCallback, 
								    ClientUserData user, string password, 
									ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable,
									Action<string> errorCallback) 
			: base(errorCallback)
			
		{
			this.sessionIdVariable = sessionIdVariable;
			this.user = user;
			this.password = password;
			this.loginSuccessfulCallback = loginSuccessfulCallback;
		}
		
				
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<LoginRequest, LoginResponse>(
				new LoginRequest(sessionIdVariable.Value, user.Id, password),
				socket,				
				loginResponse => loginSuccessfulCallback()
			);
		}
	}
}
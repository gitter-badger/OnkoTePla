using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class LogoutRequestHandler : RequestHandlerBase
	{
		private readonly ClientUserData user;
		private readonly ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable;
		private readonly Action logoutSuccessfulCallback;
		

		public LogoutRequestHandler (Action logoutSuccessfulCallback, 
								     ClientUserData user,
									 ISharedStateReadOnly<ConnectionSessionId> sessionIdVariable,
									 Action<string> errorCallback) 
			: base(errorCallback)
		{ 
			this.user = user;
			this.sessionIdVariable = sessionIdVariable;
			this.logoutSuccessfulCallback = logoutSuccessfulCallback;
		}


		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<LogoutRequest, LogoutResponse>(
				new LogoutRequest(sessionIdVariable.Value, user.Id),
				socket,				
				logoutResponse => logoutSuccessfulCallback()
			);
		}
	}
}
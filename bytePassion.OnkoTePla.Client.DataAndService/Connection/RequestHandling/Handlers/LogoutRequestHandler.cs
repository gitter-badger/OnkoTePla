using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Config;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class LogoutRequestHandler : RequestHandlerBase
	{
		private readonly ClientUserData user;
		private readonly ISharedState<ConnectionInfo> connectionInfoVariable;
		private readonly Action logoutSuccessfulCallback;
		

		public LogoutRequestHandler (Action logoutSuccessfulCallback, 
								     ClientUserData user,
									 ISharedState<ConnectionInfo> connectionInfoVariable,
									 Action<string> errorCallback) 
			: base(errorCallback)
		{ 
			this.user = user;
			this.connectionInfoVariable = connectionInfoVariable;
			this.logoutSuccessfulCallback = logoutSuccessfulCallback;
		}


		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<LogoutRequest, LogoutResponse>(
				new LogoutRequest(connectionInfoVariable.Value.SessionId, user.Id),
				socket,
				logoutResponse =>
				{
					connectionInfoVariable.Value = new ConnectionInfo(connectionInfoVariable.Value.SessionId, null);
					logoutSuccessfulCallback();					
				}
			);
		}
	}
}
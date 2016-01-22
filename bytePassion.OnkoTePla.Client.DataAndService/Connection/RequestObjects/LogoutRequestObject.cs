using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class LogoutRequestObject : RequestObject
	{
		public LogoutRequestObject (Action logoutSuccessfulCallback, 
								    ClientUserData user,
								    Action<string> errorCallback) 
			: base(NetworkMessageType.LogoutRequest, errorCallback)
		{ 
			User = user;			
			LogoutSuccessfulCallback = logoutSuccessfulCallback;
		}
		
		public ClientUserData User					   { get; }		
		public Action		  LogoutSuccessfulCallback { get; }
	}
}
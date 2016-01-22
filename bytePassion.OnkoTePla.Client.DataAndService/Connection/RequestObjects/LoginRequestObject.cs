using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class LoginRequestObject : RequestObject
	{
		public LoginRequestObject(Action loginSuccessfulCallback, 
								  ClientUserData user, string password, 
								  Action<string> errorCallback) 
			: base(NetworkMessageType.LoginRequest, errorCallback)
		{
			User = user;
			Password = password;
			LoginSuccessfulCallback = loginSuccessfulCallback;
		}
		
		public ClientUserData User					  { get; }
		public string		  Password				  { get; }
		public Action		  LoginSuccessfulCallback { get; }
	}
}
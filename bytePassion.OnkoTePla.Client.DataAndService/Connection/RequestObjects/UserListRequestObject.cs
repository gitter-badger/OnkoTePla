using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class UserListRequestObject : RequestObject
	{
		public UserListRequestObject(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback, 
									 Action<string> errorCallback) 
			: base(NetworkMessageType.GetUserListRequest, errorCallback)
		{
			DataReceivedCallback = dataReceivedCallback;			
		}

		public Action<IReadOnlyList<ClientUserData>> DataReceivedCallback { get; }		
	}
}

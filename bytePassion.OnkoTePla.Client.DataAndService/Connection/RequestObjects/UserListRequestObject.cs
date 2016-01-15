using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;

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

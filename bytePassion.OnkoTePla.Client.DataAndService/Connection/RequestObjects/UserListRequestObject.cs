using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class UserListRequestObject : RequestObject
	{
		public UserListRequestObject(Action<IReadOnlyList<ClientUserData>> dataReceivedCallback) 
			: base(NetworkMessageType.GetUserListRequest)
		{
			DataReceivedCallback = dataReceivedCallback;
		}

		public Action<IReadOnlyList<ClientUserData>> DataReceivedCallback { get; }
	}
}

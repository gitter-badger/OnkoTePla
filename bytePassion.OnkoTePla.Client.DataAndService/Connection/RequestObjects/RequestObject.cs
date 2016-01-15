using System;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	public abstract class RequestObject
	{
		protected RequestObject(NetworkMessageType requestType, 
								Action<string> errorCallback)
		{
			RequestType = requestType;
			ErrorCallback = errorCallback;
		}

		public NetworkMessageType RequestType { get; }

		public Action<string> ErrorCallback { get; }
	}
}

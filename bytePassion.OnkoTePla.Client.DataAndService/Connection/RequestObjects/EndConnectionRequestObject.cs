using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class EndConnectionRequestObject : RequestObject
	{ 
		public EndConnectionRequestObject (Action connectionEndedCallback,											 
			Action<string> errorCallback)
			: base(NetworkMessageType.EndConnectionRequest, errorCallback)
		{
			ConnectionEndedCallback = connectionEndedCallback;
		}
		
		public Action ConnectionEndedCallback { get; }
	}
}
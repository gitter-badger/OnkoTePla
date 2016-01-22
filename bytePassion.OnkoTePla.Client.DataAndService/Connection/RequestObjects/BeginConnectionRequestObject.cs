using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects
{
	internal class BeginConnectionRequestObject : RequestObject
	{
		public BeginConnectionRequestObject (Action<ConnectionSessionId> connectionSuccessfulCallback,
											 AddressIdentifier clientAddress, 
								             Action<string> errorCallback)  
			: base(NetworkMessageType.BeginConnectionRequest, errorCallback)
		{
			ClientAddress = clientAddress;
			ConnectionSuccessfulCallback = connectionSuccessfulCallback;
		}

		public AddressIdentifier           ClientAddress                { get; }
		public Action<ConnectionSessionId> ConnectionSuccessfulCallback { get; }
	}
}
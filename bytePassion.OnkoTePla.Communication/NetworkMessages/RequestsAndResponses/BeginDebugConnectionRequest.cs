using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class BeginDebugConnectionRequest : NetworkMessageBase
	{		
		public BeginDebugConnectionRequest (AddressIdentifier clientAddress)
			: base(NetworkMessageType.BeginDebugConnectionRequest)
		{
			ClientAddress = clientAddress;
		}
		
		public AddressIdentifier ClientAddress { get; }

		public override string AsString()
		{
			return $"{ClientAddress}";
        }
		
		public static BeginDebugConnectionRequest Parse (string s)
		{			
			var clientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(s);
			return new BeginDebugConnectionRequest(clientAddress);
		}
	}
}

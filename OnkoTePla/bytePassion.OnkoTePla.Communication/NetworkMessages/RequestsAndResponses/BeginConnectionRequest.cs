using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class BeginConnectionRequest : NetworkMessageBase
	{		
		public BeginConnectionRequest(AddressIdentifier clientAddress)
			: base(NetworkMessageType.BeginConnectionRequest)
		{
			ClientAddress = clientAddress;
		}

		public AddressIdentifier ClientAddress { get; }

		public override string AsString()
		{
			return $"{ClientAddress}";
        }

		public static BeginConnectionRequest Parse(string s)
		{			
			var clientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(s);
			return new BeginConnectionRequest(clientAddress);
		}
	}
}

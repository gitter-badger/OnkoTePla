using System;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.Connection
{
	public class Request
	{
		public Request(AddressIdentifier clientAddress)
		{
			ClientAddress = clientAddress;
		}

		public AddressIdentifier ClientAddress { get; }

		public string AsString()
		{
			return $"ConnectionRequest;{ClientAddress}";
		}

		public static Request Parse(string s)
		{
			var parts = s.Split(';');

			if (parts.Length != 2)
				throw new ArgumentException($"{s} is not a connectionRequest");

			if (parts[0] != "ConnectionRequest")
				throw new ArgumentException($"{s} is not a connectionRequest");

			var clientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(parts[1]);

			return new Request(clientAddress);
		}
	}
}

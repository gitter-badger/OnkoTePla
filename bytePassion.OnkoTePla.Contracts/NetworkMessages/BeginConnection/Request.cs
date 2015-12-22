using System;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Contracts.NetworkMessages.BeginConnection
{
	public class Request
	{
		private const string MsgIdentifier = "ConnectionBeginRequest";

		public Request(AddressIdentifier clientAddress)
		{
			ClientAddress = clientAddress;
		}

		public AddressIdentifier ClientAddress { get; }

		public string AsString()
		{
			return $"{MsgIdentifier};{ClientAddress}";
        }

		public static Request Parse(string s)
		{
			var parts = s.Split(';');

			if (parts.Length != 2)
				throw new ArgumentException($"{s} is not a {MsgIdentifier}");

			if (parts[0] != MsgIdentifier)
				throw new ArgumentException($"{s} is not a {MsgIdentifier }");

			var clientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(parts[1]);
			return new Request(clientAddress);
		}
	}
}

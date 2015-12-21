using System.Collections.Generic;
using System.Net;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.Lib.ZmqUtils
{
	public static class IpAddressCatcher
	{
		public static IReadOnlyList<Address> GetAllAvailableLocalIpAddresses ()
		{
			string strHostName = Dns.GetHostName();
			var iphostentry = Dns.GetHostByName(strHostName);

			var addressList = new List<Address>();

			foreach (var ipaddress in iphostentry.AddressList)
			{
				addressList.Add(new Address(new TcpIpProtocol(),
											AddressIdentifier.GetIpAddressIdentifierFromString(ipaddress.ToString())));
			}

			if (addressList.Count == 0)
				addressList.Add(new Address(new TcpIpProtocol(), new IpV4AddressIdentifier(127, 0, 0, 1)));

			return addressList;
		}
	}
}

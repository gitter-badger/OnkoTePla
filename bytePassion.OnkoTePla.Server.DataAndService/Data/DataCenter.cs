using System.Collections.Generic;
using System.Net;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Resources;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	internal class DataCenter : IDataCenter
	{
		public DataCenter()
		{
			
		}


		public IReadOnlyList<Address> GetAllAvailableAddresses()
		{			
			string strHostName = Dns.GetHostName();						
			var iphostentry = Dns.GetHostEntry(strHostName);

			var addressList = new List<Address>();
						
			foreach (var ipaddress in iphostentry.AddressList)
			{
				addressList.Add(new Address(new TcpIpProtocol(), 
											AddressIdentifier.GetIpAddressIdentifierFromString(ipaddress.ToString() + ":" + GlobalConstants.TcpIpPort)));				
			}

			if (addressList.Count == 0)
				addressList.Add(new Address(new TcpIpProtocol(), new IpV4AddressIdentifier(127,0,0,1,new IpPort(GlobalConstants.TcpIpPort))));

			return addressList;			
		}
	}
}

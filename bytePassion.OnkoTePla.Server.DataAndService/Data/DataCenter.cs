using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	internal class DataCenter : IDataCenter
	{
		public DataCenter()
		{
			
		}


		public IReadOnlyList<Address> GetAllAvailableAddresses()
		{
			return IpAddressCatcher.GetAllAvailableLocalIpAddresses();
		}
	}
}

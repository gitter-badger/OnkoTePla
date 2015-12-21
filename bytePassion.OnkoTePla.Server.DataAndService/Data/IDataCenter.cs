using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	public interface IDataCenter
	{		
		IReadOnlyList<Address> GetAllAvailableAddresses();
	}
}
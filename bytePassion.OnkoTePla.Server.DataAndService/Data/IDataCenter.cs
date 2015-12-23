using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	public interface IDataCenter
	{		
		IReadOnlyList<Address> GetAllAvailableAddresses();

		IEnumerable<User> GetAllUsers();
		void AddNewUser(User newUser);
		void UpdateUser(User updatedUser);		
	}
}
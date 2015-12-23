using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Repositories.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	internal class DataCenter : IDataCenter
	{
		private readonly IConfigurationReadRepository readConfig;
		private readonly IConfigurationWriteRepository writeConfig;

		public DataCenter(IConfigurationReadRepository readConfig,
						  IConfigurationWriteRepository writeConfig)
		{
			this.readConfig = readConfig;
			this.writeConfig = writeConfig;
		}

		

		public IReadOnlyList<Address> GetAllAvailableAddresses()
		{
			return IpAddressCatcher.GetAllAvailableLocalIpAddresses();
		}

		#region users

		public IEnumerable<User> GetAllUsers()
		{
			return readConfig.GetAllUsers();
		}

		public void AddNewUser(User newUser)
		{
			writeConfig.AddUser(newUser);
		}

		public void UpdateUser(User updatedUser)
		{
			writeConfig.UpdateUser(updatedUser);
		}

		#endregion

		#region therapyPlaceType

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			return readConfig.GetAllTherapyPlaceTypes();
		}

		public void AddNewTherapyPlaceType(TherapyPlaceType newTherapyPlaceType)
		{
			writeConfig.AddTherapyPlaceType(newTherapyPlaceType);
		}

		public void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType)
		{ 
			writeConfig.UpdateTherapyPlaceType(updatedTherapyPlaceType);
		}

		#endregion
	}
}

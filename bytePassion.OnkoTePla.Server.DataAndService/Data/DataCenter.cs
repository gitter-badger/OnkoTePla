using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
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

		#region therapyPlaceTypes

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			return readConfig.GetAllTherapyPlaceTypes()
							 .Append(TherapyPlaceType.NoType);
		}

		public void AddNewTherapyPlaceType(TherapyPlaceType newTherapyPlaceType)
		{
			writeConfig.AddTherapyPlaceType(newTherapyPlaceType);
		}

		public void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType)
		{ 
			writeConfig.UpdateTherapyPlaceType(updatedTherapyPlaceType);
		}

		public TherapyPlaceType GetTherapyPlaceType(Guid id)
		{
			return readConfig.GetTherapyPlaceTypeById(id);
		}

		#endregion

		#region medicalPractices

		public IEnumerable<MedicalPractice> GetAllMedicalPractices ()
		{
			return readConfig.GetAllMedicalPractices();
		}

		public void AddNewMedicalPractice (MedicalPractice newMedicalPractice)
		{
			writeConfig.AddMedicalPractice(newMedicalPractice);
		}

		public void UpdateMedicalPractice (MedicalPractice updatedMedicalPractice)
		{
			writeConfig.RemoveMedicalPractice(updatedMedicalPractice.Id);
			writeConfig.AddMedicalPractice(updatedMedicalPractice);
		}

		public void RemoveMedicalPractice(MedicalPractice medicalPracticeToRemove)
		{
			writeConfig.RemoveMedicalPractice(medicalPracticeToRemove.Id);
		}

		public MedicalPractice GetMedicalPractice(Guid id)
		{
			return readConfig.GetMedicalPracticeById(id);
		}

		#endregion
	}
}

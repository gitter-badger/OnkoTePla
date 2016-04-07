using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores.JsonSerializationDoubles
{
	class ConfigurationSerializationDouble
	{

		public ConfigurationSerializationDouble()
		{			
		}

		public ConfigurationSerializationDouble (Configuration configuration)
		{
			ConfiguredMedicalPractices  = configuration.GetAllMedicalPractices().Select(medicalPractice => new MedicalPracticeSerializationDouble(medicalPractice));
			ConfiguredTherapyPlaceTypes = configuration.GetAllTherapyPlaceTypes().Select(therapyPlaceType => new TherapyPlaceTypeSerializationDouble(therapyPlaceType));
			ConfiguredUsers             = configuration.GetAllUsers().Select(user => new UserSerializationDouble(user));
		}	
		
		public IEnumerable<TherapyPlaceTypeSerializationDouble> ConfiguredTherapyPlaceTypes { get; set; }
		public IEnumerable<MedicalPracticeSerializationDouble>  ConfiguredMedicalPractices  { get; set; }
		public IEnumerable<UserSerializationDouble>             ConfiguredUsers             { get; set; }

		public Configuration GetConfiguration()
		{
			return new Configuration(ConfiguredTherapyPlaceTypes.Select(therapyPlaceTypeDouble => therapyPlaceTypeDouble.GetTherapyPlaceType()), 
									 ConfiguredMedicalPractices.Select(medicalPracticeDouble => medicalPracticeDouble.GetMedicalPractice()), 
									 ConfiguredUsers.Select(userDouble => userDouble.GetUser()));
		}	
	}
}

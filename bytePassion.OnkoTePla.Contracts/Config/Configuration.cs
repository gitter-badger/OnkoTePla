using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class Configuration
	{
		private readonly IEnumerable<TherapyPlaceType> configuredTherapyPlaceTypes;
		private readonly IEnumerable<MedicalPractice>  configuredMedicalPractices;
		private readonly IEnumerable<User>			   configuredUsers; 
		

		public Configuration(IEnumerable<TherapyPlaceType> configuredTherapyPlaceTypes, 
							 IEnumerable<MedicalPractice> configuredMedicalPractices, 
							 IEnumerable<User> configuredUsers)
		{			
			this.configuredMedicalPractices  = configuredMedicalPractices.ToList();
			this.configuredTherapyPlaceTypes = configuredTherapyPlaceTypes.ToList();
			this.configuredUsers = configuredUsers.ToList();
		}		

		public TherapyPlaceType GetTherapyPlaceTypeByName(string name)
		{
			 return configuredTherapyPlaceTypes.FirstOrDefault(therapyPlace => therapyPlace.Name == name);			
		}

		public MedicalPractice GetMedicalPracticeByName(string name)
		{
			return configuredMedicalPractices.FirstOrDefault(medicalPractice => medicalPractice.Name == name);
		}

		public User GetUserByName(string name)
		{
			return configuredUsers.FirstOrDefault(user => user.Name == name);
		}

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			return configuredTherapyPlaceTypes.ToList();
		}

		public IEnumerable<MedicalPractice> GetAllMedicalPractices()
		{
			return configuredMedicalPractices.ToList();
		}

		public IEnumerable<User> GetAllUsers()
		{
			return configuredUsers.ToList();
		} 
	} 
}

using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Contracts.Config
{

	public class Configuration
	{
        private readonly IList<TherapyPlaceType> configuredTherapyPlaceTypes;
        private readonly IList<MedicalPractice>  configuredMedicalPractices;
        private readonly IList<User>			 configuredUsers; 
		

		public Configuration(IEnumerable<TherapyPlaceType> configuredTherapyPlaceTypes, 
							 IEnumerable<MedicalPractice> configuredMedicalPractices, 
							 IEnumerable<User> configuredUsers)
		{			
			this.configuredMedicalPractices = configuredMedicalPractices.ToList();
			this.configuredTherapyPlaceTypes = configuredTherapyPlaceTypes.ToList();
			this.configuredUsers = configuredUsers.ToList();
		}

		#region TherapyPlaceTypes

		public TherapyPlaceType GetTherapyPlaceTypeByName(string name)
		{
			 return configuredTherapyPlaceTypes.FirstOrDefault(therapyPlace => therapyPlace.Name == name);			
		}

		public TherapyPlaceType GetTherapyPlaceTypeById(Guid id)
		{
			return configuredTherapyPlaceTypes.FirstOrDefault(therapyPlace => therapyPlace.Id == id);
		}

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			return configuredTherapyPlaceTypes.ToList();
		}

		public void AddTherapyPlaceType(TherapyPlaceType newTherapyPlaceType)
		{
			configuredTherapyPlaceTypes.Add(newTherapyPlaceType);
		}

		public void UpdateTherapyPlaceTupe (TherapyPlaceType updatedTherapyPlaceType)
		{
			configuredTherapyPlaceTypes.Remove(GetTherapyPlaceTypeById(updatedTherapyPlaceType.Id));
			AddTherapyPlaceType(updatedTherapyPlaceType);
		}

		#endregion


		#region MedicalPractice

		public MedicalPractice GetMedicalPracticeByName(string name)
		{
			return configuredMedicalPractices.FirstOrDefault(medicalPractice => medicalPractice.Name == name);
		}

		public MedicalPractice GetMedicalPracticeById(Guid id)
		{
			return configuredMedicalPractices.FirstOrDefault(medicalPractice => medicalPractice.Id == id);
		}

		public MedicalPractice GetMedicalPracticeByIdAndVersion (Guid id, uint version)
		{
			var medicalPractice = GetMedicalPracticeById(id);
			return medicalPractice.GetVersion(version);
		}

		public IEnumerable<MedicalPractice> GetAllMedicalPractices ()
		{
			return configuredMedicalPractices.ToList();
		}

		public void AddMedicalPractice(MedicalPractice practice)
		{
			configuredMedicalPractices.Add(practice);
		}

		public void RemoveMedicalPractice(Guid medicalPracticeId)
		{
			configuredMedicalPractices.Remove(GetMedicalPracticeById(medicalPracticeId));
		}

		#endregion


		#region User

		public User GetUserByName (string name)
		{
			return configuredUsers.FirstOrDefault(user => user.Name == name);
		}

		public User GetUserById(Guid id)
		{
			return configuredUsers.FirstOrDefault(user => user.Id == id);
		}

		public IEnumerable<User> GetAllUsers ()
		{
			return configuredUsers.ToList();
		} 

		public void AddUser(User newUser)
		{
			configuredUsers.Add(newUser);
		}	

		public void UpdateUser(User updatedUser)
		{
			configuredUsers.Remove(GetUserById(updatedUser.Id));
			AddUser(updatedUser);
		}

		#endregion		
	} 
}

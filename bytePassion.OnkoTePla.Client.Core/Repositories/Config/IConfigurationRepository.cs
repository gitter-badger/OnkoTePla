using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
	public interface IConfigurationRepository
	{
		uint GetLatestVersionFor(Guid medicalPractiveId);		

		MedicalPractice GetMedicalPracticeByName(string name);
		MedicalPractice GetMedicalPracticeById  (Guid id);
		IEnumerable<MedicalPractice> GetAllMedicalPractices();
		void AddMedicalPractice(MedicalPractice practice);
		void RemoveMedicalPractice(Guid medicalPracticeId);

		TherapyPlaceType GetTherapyPlaceTypeByName(string name);
		TherapyPlaceType GetTherapyPlaceTypeById  (Guid id);		
		IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes();
		void AddTherapyPlaceType(TherapyPlaceType newTherapyPlaceType);

		User GetUserByName(string name);
		User GetUserById(Guid id);
		IEnumerable<User> GetAllUsers(); 
		void AddUser(User newUser);
		void RemoveUser(Guid userId);

	}
}

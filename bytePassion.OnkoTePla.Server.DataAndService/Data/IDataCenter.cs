using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	public interface IDataCenter
	{		
		IReadOnlyList<Address> GetAllAvailableAddresses();

		IEnumerable<User> GetAllUsers();
		void AddNewUser(User newUser);
		void UpdateUser(User updatedUser);
		User GetUser(Guid id);

		IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes();
		void AddNewTherapyPlaceType(TherapyPlaceType newTherapyPlaceType);
		void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType);
		TherapyPlaceType GetTherapyPlaceType(Guid id);

		IEnumerable<MedicalPractice> GetAllMedicalPractices();
		void AddNewMedicalPractice(MedicalPractice newMedicalPractice);
		void UpdateMedicalPractice(MedicalPractice updatedMedicalPractice);
		void RemoveMedicalPractice(MedicalPractice medicalPracticeToRemove);
		MedicalPractice GetMedicalPractice(Guid id);
	}
}
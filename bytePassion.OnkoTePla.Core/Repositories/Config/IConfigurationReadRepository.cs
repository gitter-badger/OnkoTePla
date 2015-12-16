using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Core.Repositories.Config
{
    public interface IConfigurationReadRepository : IPersistable
	{
		uint GetLatestVersionFor(Guid medicalPractiveId);		

		/////////////////////////////////////////////////////////////////////////////////////
		////////                          Medical practice                          /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		MedicalPractice GetMedicalPracticeByName (string name);						/////////
		MedicalPractice GetMedicalPracticeById (Guid id);							/////////
		MedicalPractice GetMedicalPracticeByIdAndVersion (Guid id, uint version);	/////////																				
		IEnumerable<MedicalPractice> GetAllMedicalPractices ();						/////////
																					/////////	
		/////////////////////////////////////////////////////////////////////////////////////
		


		/////////////////////////////////////////////////////////////////////////////////////
		////////                            Therapy Place                           /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		TherapyPlaceType GetTherapyPlaceTypeByName (string name);					/////////
		TherapyPlaceType GetTherapyPlaceTypeById (Guid id);							/////////
		IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes ();					/////////
																					/////////
		/////////////////////////////////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////////////////////////////////
		////////                                 User                               /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		User GetUserByName (string name);											/////////
		User GetUserById (Guid id);													/////////
		IEnumerable<User> GetAllUsers ();											/////////
																					/////////																					
		/////////////////////////////////////////////////////////////////////////////////////
	}
}

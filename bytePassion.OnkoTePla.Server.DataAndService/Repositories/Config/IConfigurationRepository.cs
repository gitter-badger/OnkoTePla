using System;
using System.Collections.Generic;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.Config
{
	public interface IConfigurationRepository : IPersistable
	{		
		/////////////////////////////////////////////////////////////////////////////////////
		////////                          Medical practice                          /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////		
		MedicalPractice GetMedicalPracticeById (Guid id);							/////////
		MedicalPractice GetMedicalPracticeByIdAndVersion (Guid id, uint version);	/////////																				
		IEnumerable<MedicalPractice> GetAllMedicalPractices ();						/////////
		uint GetLatestVersionFor(Guid medicalPractiveId);							/////////
																					/////////
		void AddMedicalPractice   (MedicalPractice practice);						/////////
		void RemoveMedicalPractice(Guid medicalPracticeId);							/////////
																					/////////	
		/////////////////////////////////////////////////////////////////////////////////////
		


		/////////////////////////////////////////////////////////////////////////////////////
		////////                           TherapyPlaceType                         /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////		
		TherapyPlaceType GetTherapyPlaceTypeById (Guid id);							/////////
		IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes ();                   /////////
																					/////////
		void AddTherapyPlaceType   (TherapyPlaceType newTherapyPlaceType);			/////////
		void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType);		/////////
																					/////////
		/////////////////////////////////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////////////////////////////////
		////////                                 User                               /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		User GetUserById (Guid id);													/////////
		IEnumerable<User> GetAllUsers ();                                           /////////
																					/////////
		void AddUser    (User newUser);												/////////
		void UpdateUser (User updatedUser);											/////////
																					/////////																					
		/////////////////////////////////////////////////////////////////////////////////////
		


		/////////////////////////////////////////////////////////////////////////////////////
		////////                                 Label                              /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		Label GetLabelById (Guid id);												/////////
		IEnumerable<Label> GetAllLabels ();                                         /////////
																					/////////
		void AddLabel    (Label newLabel);											/////////
		void UpdateLabel (Label updatedLabel);										/////////
																					/////////																					
		/////////////////////////////////////////////////////////////////////////////////////
	}
}

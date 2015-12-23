﻿using System;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Core.Repositories.Config
{
	public interface IConfigurationWriteRepository : IPersistable
	{
		
		/////////////////////////////////////////////////////////////////////////////////////
		////////                          Medical practice                          /////////
		/////////////////////////////////////////////////////////////////////////////////////																							
																					/////////
		void AddMedicalPractice (MedicalPractice practice);							/////////
		void RemoveMedicalPractice (Guid medicalPracticeId);						/////////
																					/////////
		/////////////////////////////////////////////////////////////////////////////////////
		


		/////////////////////////////////////////////////////////////////////////////////////
		////////                            Therapy Place                           /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		void AddTherapyPlaceType   (TherapyPlaceType newTherapyPlaceType);			/////////
		void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType);		/////////
																					/////////
		/////////////////////////////////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////////////////////////////////
		////////                                 User                               /////////
		/////////////////////////////////////////////////////////////////////////////////////
																					/////////
		void AddUser    (User newUser);												/////////
		void UpdateUser (User updatedUser);											/////////
																					/////////
		/////////////////////////////////////////////////////////////////////////////////////
	}
}

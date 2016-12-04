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
		private readonly IList<Label>			 configuredLabels; 
		

		public Configuration(IEnumerable<TherapyPlaceType> configuredTherapyPlaceTypes, 
							 IEnumerable<MedicalPractice> configuredMedicalPractices, 
							 IEnumerable<User> configuredUsers, 
							 IEnumerable<Label> configuredLabels)
		{			
			this.configuredMedicalPractices = configuredMedicalPractices.ToList();
			this.configuredTherapyPlaceTypes = configuredTherapyPlaceTypes.ToList();
			this.configuredUsers = configuredUsers.ToList();
			this.configuredLabels = configuredLabels.ToList();
		}

		#region TherapyPlaceTypes		

		public TherapyPlaceType GetTherapyPlaceTypeById(Guid id)
		{
			if (id == Guid.Empty)
				return TherapyPlaceType.NoType;

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


		#region Labels

		public Label GetLabelById(Guid id)
		{
			return configuredLabels.FirstOrDefault(label => label.Id == id);
		}

		public IEnumerable<Label> GetAllLabels()
		{
			return configuredLabels.ToList();
		}

		public void AddLabel(Label newLabel)
		{
			configuredLabels.Add(newLabel);
		}

		public void UpdateLabel(Label updatedLabel)
		{
			configuredLabels.Remove(GetLabelById(updatedLabel.Id));
			AddLabel(updatedLabel);
		}

		#endregion
	} 
}

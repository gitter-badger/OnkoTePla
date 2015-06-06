using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
	public interface IConfigurationRepository
	{
		void SetConfig(Configuration newConfig);

		MedicalPractice GetMedicalPracticeByName(string name);
		TherapyPlaceType GetTherapyPlaceTypeByName(string name);

		IEnumerable<MedicalPractice> GetAllMedicalPractices();
		IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes();
	}
}

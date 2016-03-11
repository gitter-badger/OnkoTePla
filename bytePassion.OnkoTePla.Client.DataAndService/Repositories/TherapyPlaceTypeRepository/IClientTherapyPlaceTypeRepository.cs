using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.TherapyPlaceTypeRepository
{
	public interface IClientTherapyPlaceTypeRepository : IDisposable
	{
		event Action<TherapyPlaceType> UpdatedTherapyPlaceTypeAvailable;
			
		void RequestTherapyPlaceTypes(Action<TherapyPlaceType> therapyPlacetypesAvailableCallback, Guid id , Action<string> errorCallback);
	}
}

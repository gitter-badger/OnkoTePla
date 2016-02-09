using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.TherapyPlaceTypeRepository
{
	public interface IClientTherapyPlaceTypeRepository
	{				
		void RequestTherapyPlaceTypes(Action<TherapyPlaceType> therapyPlacetypesAvailableCallback, Guid id , Action<string> errorCallback);
	}
}

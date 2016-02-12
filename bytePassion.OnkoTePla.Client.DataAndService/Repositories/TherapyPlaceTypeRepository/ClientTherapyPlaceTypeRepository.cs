using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.TherapyPlaceTypeRepository
{
	public class ClientTherapyPlaceTypeRepository : IClientTherapyPlaceTypeRepository
	{		
		private readonly IConnectionService connectionService;
		private  IDictionary<Guid, TherapyPlaceType> cachedTherapyPlaceTypes; 


		public ClientTherapyPlaceTypeRepository (IConnectionService connectionService)
		{
			this.connectionService = connectionService;			
		}																
		 				

		public void RequestTherapyPlaceTypes(Action<TherapyPlaceType> therapyPlacetypesAvailableCallback, Guid id, Action<string> errorCallback)
		{
			if (cachedTherapyPlaceTypes == null)
			{

				connectionService.RequestTherapyPlaceTypeList(
					therapyPlaceTypeList =>
					{
						cachedTherapyPlaceTypes = new Dictionary<Guid, TherapyPlaceType>();
						foreach (var therapyPlaceType in therapyPlaceTypeList)
						{
							if (!cachedTherapyPlaceTypes.ContainsKey(therapyPlaceType.Id))
								cachedTherapyPlaceTypes.Add(therapyPlaceType.Id, therapyPlaceType);
						}

						therapyPlacetypesAvailableCallback(GetTherapyPlaceType(id, errorCallback));
					},
					errorCallback
					);
			}
			else
			{
				therapyPlacetypesAvailableCallback(GetTherapyPlaceType(id, errorCallback));
			}
		}

		private TherapyPlaceType GetTherapyPlaceType (Guid id, Action<string> errorCallback)
		{
			if (cachedTherapyPlaceTypes.ContainsKey(id))
				return cachedTherapyPlaceTypes[id];

			errorCallback("therpyPlaceType not found");
			return null;
		}
	}
}
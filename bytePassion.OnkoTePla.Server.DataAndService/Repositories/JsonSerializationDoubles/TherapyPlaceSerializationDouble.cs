using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles
{
	public class TherapyPlaceSerializationDouble
	{
		public TherapyPlaceSerializationDouble()
		{			
		}

		public TherapyPlaceSerializationDouble(TherapyPlace therapyPlace)
		{
			Id     = therapyPlace.Id;
			Name   = therapyPlace.Name;
			TypeId = therapyPlace.TypeId;
		}

		public Guid   Id     { get; set; }
		public string Name   { get; set; }
		public Guid   TypeId { get; set; }

		public TherapyPlace GetTherapyPlace()
		{
			return new TherapyPlace(Id, TypeId, Name);
		}
	}
}
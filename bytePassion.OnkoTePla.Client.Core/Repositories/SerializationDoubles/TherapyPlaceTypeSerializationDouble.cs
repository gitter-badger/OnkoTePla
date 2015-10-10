using System;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles
{
	internal class TherapyPlaceTypeSerializationDouble
	{
		public TherapyPlaceTypeSerializationDouble()
		{			
		}

		public TherapyPlaceTypeSerializationDouble (TherapyPlaceType therapyPlaceType)
		{
			Name     = therapyPlaceType.Name;
			IconType = therapyPlaceType.IconType;
			Id       = therapyPlaceType.Id;
		}

		public string               Name     { get; set; }
		public TherapyPlaceIconType IconType { get; set; }
		public Guid                 Id       { get; set; }

		public TherapyPlaceType GetTherapyPlaceType()
		{
			return new TherapyPlaceType(Name, IconType, Id);
		}
	}
}
using System;

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public static class TherapyPlaceTypeCreateAndEditLogic
	{
		public static TherapyPlaceType Create()
		{
			return new TherapyPlaceType("noName", TherapyPlaceTypeIcon.None, Guid.NewGuid());
		}

		public static TherapyPlaceType SetNewName(this TherapyPlaceType therapyPlaceType, string newName)
		{
			return new TherapyPlaceType(newName,
				therapyPlaceType.IconType,
				therapyPlaceType.Id);
		}

		public static TherapyPlaceType SetNewIcon(this TherapyPlaceType therapyPlaceType, TherapyPlaceTypeIcon newIcon)
		{
			return new TherapyPlaceType(therapyPlaceType.Name,
				newIcon,
				therapyPlaceType.Id);
		}
	}
}
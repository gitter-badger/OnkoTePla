using System;

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public static class TherapyPlaceCreateAndEditLogic
	{
		public static TherapyPlace Create(string name)
		{
			return new TherapyPlace(Guid.NewGuid(),
									TherapyPlaceType.NoType.Id,
									name);
		}

		public static TherapyPlace SetNewName(this TherapyPlace therapyPlace, string newName)
		{
			return new TherapyPlace(therapyPlace.Id,
									therapyPlace.TypeId,
									newName);
		}

		public static TherapyPlace SetNewType(this TherapyPlace therapyPlace, Guid newTherapyPlaceTypeId)
		{
			return new TherapyPlace(therapyPlace.Id,
									newTherapyPlaceTypeId,
									therapyPlace.Name);
		}
	}
}

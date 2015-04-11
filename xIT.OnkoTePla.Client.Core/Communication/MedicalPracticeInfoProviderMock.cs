using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Communication;
using xIT.OnkoTePla.Contracts.DataObjects;
using xIT.OnkoTePla.Contracts.Enums;


namespace xIT.OnkoTePla.Client.Core.Communication
{
	public class MedicalPracticeInfoProviderMock : IMedicalPracticeInfoProvider
	{
		
		public MedicalPractice GetMedicalPractice()
		{
			TherapyPlaceType chair = new TherapyPlaceType("chair", TherapyPlaceIconType.ChairType1);
			TherapyPlaceType bed   = new TherapyPlaceType("bed", TherapyPlaceIconType.BedType1);

			var listOfTherapyPlacesRoom1 = new List<TherapyPlace>()
			{
				new TherapyPlace(0, chair),
				new TherapyPlace(1, chair),
				new TherapyPlace(2, chair),
				new TherapyPlace(3, bed),
				new TherapyPlace(4, bed)
			};
			

			var listOfTherapyPlacesRoom2 = new List<TherapyPlace>()
			{
				new TherapyPlace(5, chair),
				new TherapyPlace(6, chair),
				new TherapyPlace(7, chair),
				new TherapyPlace(8, chair),
				new TherapyPlace(9, chair)
			};
			

			return new MedicalPractice(new List<Room>()
			{
				new Room(0, listOfTherapyPlacesRoom1),
				new Room(0, listOfTherapyPlacesRoom2)
			});			
		}
	}
}

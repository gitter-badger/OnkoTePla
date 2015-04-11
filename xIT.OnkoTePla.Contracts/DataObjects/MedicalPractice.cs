using System.Collections.Generic;
using System.Linq;

namespace xIT.OnkoTePla.Contracts.DataObjects
{
	public class MedicalPractice
	{
		private readonly IReadOnlyList<Room> rooms;

		public MedicalPractice(IReadOnlyList<Room> rooms)
		{
			this.rooms = rooms;
		}

		public IReadOnlyList<Room> Rooms
		{
			get { return rooms; }
		}

		public IReadOnlyList<TherapyPlace> AllTherapyPlaces
		{
			get
			{
				return Rooms.SelectMany(room => room.AvailableTherapyPlaces)
							.ToList();
			}
		} 
	}
}

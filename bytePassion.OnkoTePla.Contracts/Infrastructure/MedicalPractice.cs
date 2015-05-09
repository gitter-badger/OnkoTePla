using System.Collections.Generic;
using System.Linq;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class MedicalPractice
	{
		private readonly IReadOnlyList<Room> rooms;
		private readonly string name;

		public MedicalPractice(IReadOnlyList<Room> rooms, string name)
		{
			this.rooms = rooms;
			this.name = name;
		}

		public IReadOnlyList<Room> Rooms
		{
			get { return rooms; }
		}

		public string Name
		{
			get { return name; }
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

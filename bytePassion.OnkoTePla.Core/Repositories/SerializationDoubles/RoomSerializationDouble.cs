using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles
{
    public class RoomSerializationDouble
	{
		public RoomSerializationDouble()
		{			
		}

		public RoomSerializationDouble(Room room)
		{
			Id = room.Id;
			Name = room.Name;
			DisplayedColor = room.DisplayedColor;
			TherapyPlaces = room.TherapyPlaces.Select(therapyPlace => new TherapyPlaceSerializationDouble(therapyPlace));
		}

		public Guid   Id             { get; set; }
		public string Name           { get; set; }
		public Color  DisplayedColor { get; set; }

		public IEnumerable<TherapyPlaceSerializationDouble> TherapyPlaces { get; set; }

		public Room GetRoom()
		{
			return new Room(Id, 
							Name,
							TherapyPlaces.Select(therapyPlace => therapyPlace.GetTherapyPlace()).ToList(),
							DisplayedColor);
		}
	}
}
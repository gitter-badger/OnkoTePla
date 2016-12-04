using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public static class RoomCreateAndEditLogic
	{
		public static Room Create(string name)
		{
			return new Room(Guid.NewGuid(),
							name,
							new List<TherapyPlace>(),
							Colors.White);
		}

		public static Room SetNewName(this Room room, string newName)
		{
			return new Room(room.Id,
							newName,
							room.TherapyPlaces,
							room.DisplayedColor);
		}

		public static Room SetNewDisplayedColor(this Room room, Color newColor)
		{
			return new Room(room.Id,
							room.Name,
							room.TherapyPlaces,
							newColor);
		}

		public static Room AddTherapyPlace(this Room room, TherapyPlace newTherapyPlace)
		{
			var newTherapyPlaceList = room.TherapyPlaces.Append(newTherapyPlace);

			return new Room(room.Id,
							room.Name,
							newTherapyPlaceList,
							room.DisplayedColor);
		}

		public static Room RemoveTherapyPlace(this Room room, Guid therapyPlaceToRemove)
		{
			var newTherapyPlaceList = room.TherapyPlaces.Where(therapyPlace => therapyPlace.Id != therapyPlaceToRemove);

			return new Room(room.Id,
							room.Name,
							newTherapyPlaceList,
							room.DisplayedColor);
		}

		public static Room UpdateTherapyPlace(this Room room, TherapyPlace updatedTherapyPlace)
		{
			var oldTherapyPlaceList = room.TherapyPlaces.ToList();
			var newTherapyPlaceList = new List<TherapyPlace>();

			foreach (var therapyPlace in oldTherapyPlaceList)
			{
				if (therapyPlace.Id != updatedTherapyPlace.Id)
					newTherapyPlaceList.Add(therapyPlace);
				else
					newTherapyPlaceList.Add(updatedTherapyPlace);
			}

			return new Room(room.Id,
							room.Name,
							newTherapyPlaceList,
							room.DisplayedColor);
		}
	}
}

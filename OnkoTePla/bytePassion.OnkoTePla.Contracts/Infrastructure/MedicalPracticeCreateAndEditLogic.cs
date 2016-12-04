using System;
using System.Collections.Generic;
using System.Linq;

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public static class MedicalPracticeCreateAndEditLogic
	{
		public static MedicalPractice Create(string name)
		{
			return new MedicalPractice(new List<Room>(), 
									   name, 
									   0, 
									   Guid.NewGuid(), 
									   null, 
									   HoursOfOpening.CreateDefault());
		}
		
		public static MedicalPractice AddRoom (this MedicalPractice medPractice, Room newRoom)
		{
			var updatedRoomList = medPractice.Rooms.Concat(new List<Room> {newRoom}).ToList();
			var updatedVersion = medPractice.Version + 1;

			return new MedicalPractice(updatedRoomList, 
									   medPractice.Name, 
									   updatedVersion, 
									   medPractice.Id, 
									   medPractice, 
									   medPractice.HoursOfOpening);
		}

		public static MedicalPractice RemoveRoom (this MedicalPractice medPractice, Guid roomToRemove)
		{
			var updatedRoomList = medPractice.Rooms.Where(room => room.Id != roomToRemove).ToList();
			var updatedVersion = medPractice.Version + 1;

			return new MedicalPractice(updatedRoomList, 
									   medPractice.Name, 
									   updatedVersion, 
									   medPractice.Id, 
									   medPractice, 
									   medPractice.HoursOfOpening);
		}

		public static MedicalPractice UpdateRoom (this MedicalPractice medPractice, Room updatedRoom)
		{
			var oldRoomList = medPractice.Rooms.ToList();
			var newRoomList = new List<Room>();

			foreach (var room in oldRoomList)
			{
				if (room.Id != updatedRoom.Id)
					newRoomList.Add(room);
				else
					newRoomList.Add(updatedRoom);
			}

			var updatedVersion = medPractice.Version + 1;

			return new MedicalPractice(newRoomList, 
									   medPractice.Name, 
									   updatedVersion, 
									   medPractice.Id, 
									   medPractice, 
									   medPractice.HoursOfOpening);
		}
		
		public static MedicalPractice SetNewName (this MedicalPractice medPractice, string newName)
		{
			var updatedVersion = medPractice.Version + 1;

			return new MedicalPractice(medPractice.Rooms, 
									   newName, 
									   updatedVersion, 
									   medPractice.Id, 
									   medPractice, 
									   medPractice.HoursOfOpening);
		}

		public static MedicalPractice SetNewHoursOfOpening (this MedicalPractice medPractice, HoursOfOpening newHoursOfOpening)
		{
			var updatedVersion = medPractice.Version + 1;

			return new MedicalPractice(medPractice.Rooms, 
									   medPractice.Name, 
									   updatedVersion, 
									   medPractice.Id, 
									   medPractice, 
									   newHoursOfOpening);
		}


	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class MedicalPractice
	{
		private readonly IReadOnlyList<Room> rooms;
		private readonly string name;
		private readonly uint version;
		private readonly Guid id;
		private readonly MedicalPractice previousVersion;


		public static MedicalPractice CreateNewMedicalPractice(IReadOnlyList<Room> rooms, string name)
		{
			return new MedicalPractice(rooms, name, 0, Guid.NewGuid(), null);
		}

		public MedicalPractice(IEnumerable<Room> rooms, string name, uint version, Guid id, MedicalPractice previousVersion)
		{
			this.rooms = rooms.ToList();
			this.name = name;
			this.version = version;
			this.id = id;
			this.previousVersion = previousVersion;
		}

		public uint              Version         { get { return version;         }}
		public IEnumerable<Room> Rooms           { get { return rooms.ToList();  }}
		public string            Name            { get { return name;            }}
		public Guid              Id              { get { return id;              }}
		public MedicalPractice   PreviousVersion { get { return previousVersion; }}

		public bool HasPreviousVersion
		{
			get { return PreviousVersion != null; }
		}

		#region modify rooms

		public MedicalPractice AddRoom(Room newRoom)
		{
			var updatedRoomList = Rooms.Concat(new List<Room> {newRoom}).ToList();
			var updatedVersion = Version + 1;

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this);
		}

		public MedicalPractice RemoveRoom(Guid roomToRemove)
		{
			var updatedRoomList = Rooms.Where(room => room.Id != roomToRemove).ToList();
			var updatedVersion = Version + 1;

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this);
		}

		public MedicalPractice UpdateRoom(Guid roomToUpdate, Room newRoomVariant)
		{
			var updatedRoomList = Rooms.Where(room => room.Id != roomToUpdate)
									   .Concat(new List<Room> {newRoomVariant})
									   .ToList();
			var updatedVersion = Version + 1;

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this);
		}

		#endregion

		#region rename

		public MedicalPractice Rename(string newName)
		{
			var updatedVersion = Version + 1;

			return new MedicalPractice(Rooms, newName, updatedVersion, Id, this);
		}

		#endregion

		#region access previousVersions / therapyPlaces / Romms

		public MedicalPractice GetVersion(uint requestedVersion)
		{
			if (Version == requestedVersion)
				return this;

			if (Version < requestedVersion)
				throw new ArgumentException("the requested version does not exist");

			return previousVersion.GetVersion(requestedVersion);
		}

		public TherapyPlace GetTherapyPlaceById(Guid therapyPlaceId)
		{
			return rooms.SelectMany(room => room.TherapyPlaces)
				        .FirstOrDefault(therapyPlace => therapyPlace.Id == therapyPlaceId);
		}

		public IEnumerable<TherapyPlace> GetAllTherapyPlaces()
		{
			return rooms.SelectMany(room => room.TherapyPlaces);
		}

		public Room GetRoomForTherapyPlace(Guid therapyPlaceId)
		{
			return rooms.FirstOrDefault(room => room.TherapyPlaces.Contains(GetTherapyPlaceById(therapyPlaceId)));
		}

		public Room GetRoomById(Guid roomId)
		{
			return rooms.FirstOrDefault(room => room.Id == roomId);
		}

		#endregion

		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name + " (v:" + Version + ")";
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (med1, med2) => med1.Id == med2.Id && med1.Version == med2.Version);
		}

		public override int GetHashCode ()
		{
			return id.GetHashCode() ^ version.GetHashCode();
		}

		#endregion
	}
}

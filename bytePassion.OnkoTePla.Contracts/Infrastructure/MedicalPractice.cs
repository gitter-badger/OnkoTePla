using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
    [DataContract]
	public sealed class MedicalPractice
	{
        [DataMember(Name = "Id")]
		private readonly Guid id;
        [DataMember(Name = "Name")]
		private readonly string name;
        [DataMember(Name = "Version")]
		private readonly uint version;

        [DataMember(Name = "Rooms")]
		private readonly IReadOnlyList<Room> rooms;

        [DataMember(Name = "HoursOfOpening")]
		private readonly HoursOfOpening hoursOfOpening;

        [DataMember(Name = "PreviousVersion")]
        private readonly MedicalPractice previousVersion;


		public static MedicalPractice CreateNewMedicalPractice(IReadOnlyList<Room> rooms, string name, HoursOfOpening hoursOfOpening)
		{
			return new MedicalPractice(rooms, name, 0, Guid.NewGuid(), null, hoursOfOpening);
		}

		public MedicalPractice(IEnumerable<Room> rooms, string name, uint version, Guid id, 
							   MedicalPractice previousVersion, HoursOfOpening hoursOfOpening)
		{
			this.rooms = rooms.ToList();
			this.name = name;
			this.version = version;
			this.id = id;
			this.previousVersion = previousVersion;
			this.hoursOfOpening = hoursOfOpening;
		}

		public Guid              Id              { get { return id;              }}
		public string            Name            { get { return name;            }}
		public uint              Version         { get { return version;         }}
		public IEnumerable<Room> Rooms           { get { return rooms.ToList();  }}		
		public HoursOfOpening    HoursOfOpening  { get { return hoursOfOpening;  }}
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

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this, HoursOfOpening);
		}

		public MedicalPractice RemoveRoom(Guid roomToRemove)
		{
			var updatedRoomList = Rooms.Where(room => room.Id != roomToRemove).ToList();
			var updatedVersion = Version + 1;

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this, HoursOfOpening);
		}

		public MedicalPractice UpdateRoom(Guid roomToUpdate, Room newRoomVariant)
		{
			var updatedRoomList = Rooms.Where(room => room.Id != roomToUpdate)
									   .Concat(new List<Room> {newRoomVariant})
									   .ToList();
			var updatedVersion = Version + 1;

			return new MedicalPractice(updatedRoomList, Name, updatedVersion, Id, this, HoursOfOpening);
		}

		#endregion

		#region modify name

		public MedicalPractice Rename(string newName)
		{
			var updatedVersion = Version + 1;
			return new MedicalPractice(Rooms, newName, updatedVersion, Id, this, HoursOfOpening);
		}

		#endregion

		#region modify hoursOfOpening

		public MedicalPractice SetNewHoursOfOpening(HoursOfOpening newHoursOfOpening)
		{
			var updatedVersion = Version + 1;
			return new MedicalPractice(Rooms, Name, updatedVersion, Id, this, newHoursOfOpening);
		}

		#endregion

		#region access previousVersions / therapyPlaces / Rooms

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

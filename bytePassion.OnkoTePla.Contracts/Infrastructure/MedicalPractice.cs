using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class MedicalPractice
	{       		
		public MedicalPractice(IEnumerable<Room> rooms, string name, uint version, Guid id, 
							   MedicalPractice previousVersion, HoursOfOpening hoursOfOpening)
		{
			Rooms = rooms.ToList();
			Name = name;
			Version = version;
			Id = id;
			PreviousVersion = previousVersion;
			HoursOfOpening = hoursOfOpening;
		}

		public Guid              Id              { get; }
	    public string            Name            { get; }
	    public uint              Version         { get; }
	    public IEnumerable<Room> Rooms           { get; }
	    public HoursOfOpening    HoursOfOpening  { get; }
	    public MedicalPractice   PreviousVersion { get; }

	    public bool HasPreviousVersion => PreviousVersion != null;	    		

		#region access previousVersions / therapyPlaces / Rooms

		public MedicalPractice GetVersion(uint requestedVersion)
		{
			if (Version == requestedVersion)
				return this;

			if (Version < requestedVersion)
				throw new ArgumentException("the requested version does not exist");

			return PreviousVersion.GetVersion(requestedVersion);
		}

		public TherapyPlace GetTherapyPlaceById(Guid therapyPlaceId)
		{
			return Rooms.SelectMany(room => room.TherapyPlaces)
				        .FirstOrDefault(therapyPlace => therapyPlace.Id == therapyPlaceId);
		}

		public IEnumerable<TherapyPlace> GetAllTherapyPlaces()
		{
			return Rooms.SelectMany(room => room.TherapyPlaces);
		}

		public Room GetRoomForTherapyPlace(Guid therapyPlaceId)
		{
			return Rooms.FirstOrDefault(room => room.TherapyPlaces.Contains(GetTherapyPlaceById(therapyPlaceId)));
		}

		public Room GetRoomById(Guid roomId)
		{
			return Rooms.FirstOrDefault(room => room.Id == roomId);
		}

		#endregion

		#region ToString / HashCode / Equals

		public override string ToString    () => $"{Name} (v: {Version})";
		public override int    GetHashCode () => Id.GetHashCode() ^ Version.GetHashCode();

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (med1, med2) => med1.Id == med2.Id && med1.Version == med2.Version);
		}		

	    #endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class ClientMedicalPracticeData
	{
		public ClientMedicalPracticeData(MedicalPractice practice)
			: this(practice.Rooms, practice.Name, practice.Version, practice.Id, practice.HoursOfOpening)
		{			
		}
		   		
		public ClientMedicalPracticeData (IEnumerable<Room> rooms, string name, uint version, Guid id, 
										  HoursOfOpening hoursOfOpening)
		{
			Rooms = rooms.ToList();
			Name = name;
			Version = version;
			Id = id;			
			HoursOfOpening = hoursOfOpening;
		}

		public Guid              Id              { get; }
	    public string            Name            { get; }
	    public uint              Version         { get; }
	    public IEnumerable<Room> Rooms           { get; }
	    public HoursOfOpening    HoursOfOpening  { get; }	    	    

		#region access therapyPlaces / Rooms
		
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

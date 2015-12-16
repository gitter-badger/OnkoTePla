using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles
{
    public class MedicalPracticeSerializationDouble
	{
		public MedicalPracticeSerializationDouble()
		{			
		}

		public MedicalPracticeSerializationDouble(MedicalPractice medicalPractice)
		{
			Id             = medicalPractice.Id;
			Name           = medicalPractice.Name;
			Version        = medicalPractice.Version;
			Rooms          = medicalPractice.Rooms.Select(room => new RoomSerializationDouble(room));
			HoursOfOpening = new HoursOfOpeningSerializationDouble(medicalPractice.HoursOfOpening);

			PreviousVersion = medicalPractice.HasPreviousVersion
				? new MedicalPracticeSerializationDouble(medicalPractice.PreviousVersion)
				: null;

		}

		public Guid                                 Id              { get; set; }
		public string                               Name            { get; set; }
		public uint                                 Version         { get; set; }
		public IEnumerable<RoomSerializationDouble> Rooms           { get; set; }
		public HoursOfOpeningSerializationDouble    HoursOfOpening  { get; set; }
		public MedicalPracticeSerializationDouble   PreviousVersion { get; set; }

		public MedicalPractice GetMedicalPractice()
		{			
			return new MedicalPractice(Rooms.Select(roomDouble => roomDouble.GetRoom()), 
									   Name, 
									   Version, 
									   Id,
									   PreviousVersion?.GetMedicalPractice(), 
									   HoursOfOpening.GetHoursOfOpening());
		}
	}
}
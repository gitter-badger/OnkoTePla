using System;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles
{

	public class PatientSerializationDouble
	{									
		public PatientSerializationDouble()
		{				
		}

		public PatientSerializationDouble (Patient p)
		{
			Name       = p.Name;
			Alive      = p.Alive;
			Birthday   = new DateSerializationDouble(p.Birthday);
			Id         = p.Id;
			ExternalId = p.ExternalId;				
		}

		public string                  Name       { get; set; }
		public bool                    Alive      { get; set; }
		public DateSerializationDouble Birthday   { get; set; }
		public Guid                    Id         { get; set; }
		public string                  ExternalId { get; set; }

		public Patient GetPatient()
		{
			return new Patient(Name, 
							   Birthday.GetDate(), 
							   Alive, 
							   Id, 
							   ExternalId);
		}		
	}

}
 
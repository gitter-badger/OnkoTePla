using System;
using System.Runtime.Serialization;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles
{
	[DataContract]
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

		[DataMember(Name = "Name")]       public string                  Name       { get; set; }
		[DataMember(Name = "Alive")]      public bool                    Alive      { get; set; }
		[DataMember(Name = "Birthday")]   public DateSerializationDouble Birthday   { get; set; }
		[DataMember(Name = "Id")]         public Guid                    Id         { get; set; }
		[DataMember(Name = "ExternalId")] public string                  ExternalId { get; set; }

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
 
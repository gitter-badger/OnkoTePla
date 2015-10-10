using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.TimeLib;
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
			Birthday   = p.Birthday;
			Id         = p.Id;
			ExternalId = p.ExternalId;				
		}

		[DataMember(Name = "Name")]       public string Name       { get; set; }
		[DataMember(Name = "Alive")]      public bool   Alive      { get; set; }
		[DataMember(Name = "Birthday")]   public DateSerializationDouble   Birthday   { get; set; }
		[DataMember(Name = "Id")]         public Guid   Id         { get; set; }
		[DataMember(Name = "ExternalId")] public string ExternalId { get; set; }

		public Patient GetPatient()
		{
			return new Patient(Name, Birthday, Alive, Id, ExternalId);
		}		
	}

}

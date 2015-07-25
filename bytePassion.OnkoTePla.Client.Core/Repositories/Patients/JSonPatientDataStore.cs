using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Patients;
using Jil;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
    public class JSonPatientDataStore : IPersistenceService<IEnumerable<Patient>>
    {
		[DataContract]
		private class PatientSerializationDouble
		{
			public PatientSerializationDouble(Patient originalPatient)
			{				
				if (originalPatient != null) 
				{
					Name = originalPatient.Name;
					Alive = originalPatient.Alive;
					Birthday = originalPatient.Birthday;
					Id = originalPatient.Id;
					ExternalId = originalPatient.ExternalId;
				}
			}

			[DataMember(Name = "Name")]       public string Name       { get; set; }
			[DataMember(Name = "Alive")]      public bool   Alive      { get; set; }
			[DataMember(Name = "Birthday")]   public Date   Birthday   { get; set; }
			[DataMember(Name = "Id")]         public Guid   Id         { get; set; }
			[DataMember(Name = "ExternalId")] public string ExternalId { get; set; }

			public Patient GetPatient() => new Patient(Name, Birthday, Alive, Id, ExternalId);
		}

        private readonly string filename;

        public JSonPatientDataStore(string filename)
        {
            this.filename = filename;
        }

		public void Persist (IEnumerable<Patient> data)
		{

			var serializableData = data.Select(patient => new PatientSerializationDouble(patient));

			using (var output = new StringWriter())
			{
				JSON.Serialize(serializableData, output, Options.PrettyPrint);
				File.WriteAllText(filename, output.ToString());
			}
		}

		public IEnumerable<Patient> Load ()
		{
			List<PatientSerializationDouble> patients;

			using (var stream = new FileStream(filename, FileMode.Open))
			{
				var settings = new DataContractJsonSerializerSettings();
				var serializer = new DataContractJsonSerializer(typeof(List<PatientSerializationDouble>), settings);
				patients = (List<PatientSerializationDouble>)serializer.ReadObject(stream);
			}
			return patients.Select(patientSerializationDouble => patientSerializationDouble.GetPatient());
		}

		//        public void Persist(IEnumerable<Patient> data)
		//        {
		//
		//            using (var output = new StringWriter())
		//            {
		//                JSON.Serialize(data, output, Options.PrettyPrint);
		//                File.WriteAllText(filename,output.ToString());
		//            }
		//        }
		//
		//        public IEnumerable<Patient> Load()
		//        {
		//            var patients = new List<Patient>();
		//
		//            using (var stream = new FileStream(filename, FileMode.Open))
		//            {
		//                var settings = new DataContractJsonSerializerSettings();
		//                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Patient>), settings);
		//                patients = (List<Patient>)serializer.ReadObject(stream);
		//            }
		//            return patients;
		//
		//        }
	}
}
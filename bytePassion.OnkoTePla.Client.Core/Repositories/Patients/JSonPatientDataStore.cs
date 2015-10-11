using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles;
using bytePassion.OnkoTePla.Contracts.Patients;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.Patients
{
	public class JSonPatientDataStore : IPersistenceService<IEnumerable<Patient>>
    {		
		private readonly string filename;

        public JSonPatientDataStore(string filename)
        {
            this.filename = filename;
        }

		public void Persist (IEnumerable<Patient> data)
		{
			var serializationData = data.Select(patient => new PatientSerializationDouble(patient));

			var serializer = new JsonSerializer
			{
				Formatting = Formatting.Indented
			};

			using (var output = new StringWriter())
			{
				serializer.Serialize(output, serializationData);
				File.WriteAllText(filename, output.ToString());
			}
		}

		public IEnumerable<Patient> Load ()
		{
			List<PatientSerializationDouble> patients;
			var serializer = new JsonSerializer();

			using (StreamReader file = File.OpenText(filename))
			{
				patients = (List<PatientSerializationDouble>)serializer.Deserialize(file, typeof(List<PatientSerializationDouble>));
			}
			return patients.Select(patientDouble => patientDouble.GetPatient());
		}
	}
}
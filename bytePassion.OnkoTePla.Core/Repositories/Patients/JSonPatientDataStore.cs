using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles;
using bytePassion.OnkoTePla.Contracts.Patients;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
						
			using (var output = new StringWriter())
			{				
				new JsonSerializer().Serialize(output, serializationData);
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
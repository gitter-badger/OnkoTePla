using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores.JsonSerializationDoubles;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores
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
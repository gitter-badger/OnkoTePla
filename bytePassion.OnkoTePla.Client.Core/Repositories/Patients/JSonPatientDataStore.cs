using System.Collections.Generic;
using System.IO;
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

        public void Persist(IEnumerable<Patient> data)
        {

            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using (var output = new StringWriter())
            {
                serializer.Serialize(output, data);
                File.WriteAllText(filename, output.ToString());
            }
        }

        public IEnumerable<Patient> Load()
        {
            List<Patient> patients;
            var serializer = new JsonSerializer();

            using (StreamReader file = File.OpenText(filename))
            {
                patients = (List<Patient>)serializer.Deserialize(file, typeof(List<Patient>));
            }
            return patients;
        }
    }
}
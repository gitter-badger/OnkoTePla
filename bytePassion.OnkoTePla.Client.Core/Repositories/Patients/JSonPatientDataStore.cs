using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using bytePassion.OnkoTePla.Contracts.Patients;
using Jil;

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

            using (var output = new StringWriter())
            {
                JSON.Serialize(data, output, Options.PrettyPrint);
                File.WriteAllText(filename,output.ToString());
            }
        }

        public IEnumerable<Patient> Load()
        {
            var patients = new List<Patient>();

            using (var stream = new FileStream(filename, FileMode.Open))
            {
                var settings = new DataContractJsonSerializerSettings();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Patient>), settings);
                patients = (List<Patient>)serializer.ReadObject(stream);
            }
            return patients;

        }
    }
}
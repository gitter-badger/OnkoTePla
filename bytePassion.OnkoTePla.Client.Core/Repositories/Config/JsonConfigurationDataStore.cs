using System.IO;
using bytePassion.OnkoTePla.Contracts.Config;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
    public class JsonConfigurationDataStore : IPersistenceService<Configuration>
    {
        private readonly string fileName;

        public JsonConfigurationDataStore(string fileName)
        {
            this.fileName = fileName;
        }

        public void Persist(Configuration data)
        {


            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using (var output = new StringWriter())
            {
                serializer.Serialize(output, data);
                File.WriteAllText(fileName, output.ToString());
            }
        }

        public Configuration Load()
        {
            Configuration config;
            var serializer = new JsonSerializer();

            using (StreamReader file = File.OpenText(fileName))
            {
                config = (Configuration)serializer.Deserialize(file, typeof(Configuration));
            }
            return config;
        }
    }
}
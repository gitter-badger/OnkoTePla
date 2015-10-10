using System.IO;
using bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles;
using bytePassion.OnkoTePla.Contracts.Config;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
    public class JsonConfigurationDataStoreTest : IPersistenceService<Configuration>
    {
        private readonly string fileName;

        public JsonConfigurationDataStoreTest(string fileName)
        {
            this.fileName = fileName;
        }

        public void Persist(Configuration data)
        {
			var serializationData = new ConfigurationSerializationDouble(data);

            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };

            using (var output = new StringWriter())
            {
                serializer.Serialize(output, serializationData);
                File.WriteAllText(fileName, output.ToString());
            }
        }

        public Configuration Load()
        {
			ConfigurationSerializationDouble config;
            var serializer = new JsonSerializer();

            using (StreamReader file = File.OpenText(fileName))
            {
                config = (ConfigurationSerializationDouble)serializer.Deserialize(file, typeof(ConfigurationSerializationDouble));
            }
            return config.GetConfiguration();
        }
    }
}
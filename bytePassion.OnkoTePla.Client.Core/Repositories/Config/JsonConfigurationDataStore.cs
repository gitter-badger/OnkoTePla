using System.IO;
using bytePassion.OnkoTePla.Contracts.Config;
using Jil;

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
            using (var output = new StringWriter())
            {
                JSON.Serialize(data, output, Options.PrettyPrint);
                File.WriteAllText(fileName, output.ToString());
            }
        }

        public Configuration Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
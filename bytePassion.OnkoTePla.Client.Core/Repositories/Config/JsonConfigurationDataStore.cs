﻿using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles;
using bytePassion.OnkoTePla.Contracts.Config;
using Newtonsoft.Json;
using System.IO;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
{
    public class JsonConfigurationDataStore : IPersistenceService<Configuration>
    {
        private readonly string fileName;

        public JsonConfigurationDataStore(string fileName)
        {
            this.fileName = fileName;
        }

		public void Persist (Configuration data)
		{
			var serializationData = new ConfigurationSerializationDouble(data);
			
			using (var output = new StringWriter())
			{
				new JsonSerializer().Serialize(output, serializationData);
				File.WriteAllText(fileName, output.ToString());
			}
		}

		public Configuration Load ()
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
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class JsonEventStreamDataStore : IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>>
    {
        private readonly string filename;

        public JsonEventStreamDataStore(string filename)
        {
            this.filename = filename;
        }

		public void Persist (IEnumerable<EventStream<AggregateIdentifier>> data)
		{
			var serializationData = data.Select(eventStream => new EventStreamSerializationDouble(eventStream));		

			using (var output = new StringWriter())
			{
				new JsonSerializer().Serialize(output, serializationData);
				File.WriteAllText(filename, output.ToString());
			}
		}

		public IEnumerable<EventStream<AggregateIdentifier>> Load ()
		{
			List<EventStreamSerializationDouble> eventStreams;			

			var serializer = new JsonSerializer();

			using (StreamReader file = File.OpenText(filename))
			{
				eventStreams = (List<EventStreamSerializationDouble>)serializer.Deserialize(file, typeof(List<EventStreamSerializationDouble>));
			}						
			return eventStreams.Select(eventStreamDouble => eventStreamDouble.GetEventStream());
		}
	}
}
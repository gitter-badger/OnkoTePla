using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using bytePassion.OnkoTePla.Client.Core.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
    public class JsonEventStreamDataStore : IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>>
    {
        private readonly string filename;

        public JsonEventStreamDataStore(string filename)
        {
            this.filename = filename;
        }

        public void Persist(IEnumerable<EventStream<AggregateIdentifier>> data)
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

        public IEnumerable<EventStream<AggregateIdentifier>> Load()
        {
            List<EventStream<AggregateIdentifier>> eventStreams;
            ITraceWriter traceWriter = new MemoryTraceWriter();

            var serializer = new JsonSerializer() {TraceWriter = traceWriter};
     

            using (StreamReader file = File.OpenText(filename))
            {
                eventStreams = (List<EventStream<AggregateIdentifier>>)serializer.Deserialize(file, typeof(List<EventStream<AggregateIdentifier>>));
            }

            Debug.WriteLine(traceWriter);
            return eventStreams;
        }
    }
}
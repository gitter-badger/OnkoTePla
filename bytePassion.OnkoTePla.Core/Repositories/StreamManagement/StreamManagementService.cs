using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public class StreamManagementService : IStreamManagementService
    {
        private readonly string basePath;
        private readonly IConfigurationReadRepository config;

        public StreamManagementService(IConfigurationReadRepository config, string basePath)
        {
            this.config = config;
            this.basePath = basePath;
        }

        public EventStream<AggregateIdentifier> GetEventStream(AggregateIdentifier identifier)
        {
            throw new NotImplementedException();
        }

        public IList<EventStream<AggregateIdentifier>> GetInitialEventStreams()
        {
            foreach (var practice in config.GetAllMedicalPractices())
            {
                if (!StreamExistsForPractice(practice.Id))
                {
                    CreateInitialDirectory(practice.Id);
                }
            }
            return null;
        }

        public void SaveStreams(IList<EventStream<AggregateIdentifier>> streams)
        {
            foreach (var medicalPractice in config.GetAllMedicalPractices())
            {
                var pratciceStreams = streams.Where(st => st.Id.MedicalPracticeId == medicalPractice.Id);

                var grouped = pratciceStreams.GroupBy(s => new {s.Id.Date.Month, s.Id.Date.Year});

                foreach (var monthGroup in grouped)
                {
                    var serializationData =
                        monthGroup.Select(eventStream => new EventStreamSerializationDouble(eventStream));
                    var path = $@"{basePath}\{medicalPractice.Id}\{monthGroup.Key.Year}\{monthGroup.Key.Month}.json";

                    if (!StreamExistsForDesiredDate(monthGroup.Key.Year, monthGroup.Key.Month, medicalPractice.Id))
                    {
                        Directory.CreateDirectory($@"{basePath}\{medicalPractice.Id}\{monthGroup.Key.Year}");
                    }

                    using (var output = new StringWriter())
                    {
                        new JsonSerializer().Serialize(output, serializationData);
                        File.WriteAllText(path, output.ToString());
                    }
                }
            }
        }

        private void CreateInitialDirectory(Guid id)
        {
            var path = $@"{basePath}\{id}";

            Directory.CreateDirectory(path);
        }

        private bool StreamExistsForDesiredDate(ushort year, byte month, Guid practiceId)
        {
            var path = $@"{basePath}\{practiceId}\{year}-{month}.json";

            return File.Exists(path);
        }

        private bool StreamExistsForPractice(Guid practiceId)
        {
            var path = $@"{basePath}\{practiceId}";

            return Directory.Exists(path);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public class StreamPersistenceService : IStreamPersistenceService
    {
        private readonly string basePath;
        private readonly IConfigurationReadRepository config;

        public StreamPersistenceService(IConfigurationReadRepository config, string basePath)
        {
            this.config = config;
            this.basePath = basePath;
        }

        public EventStream<AggregateIdentifier> LoadEventStream(AggregateIdentifier identifier)
        {
            var serializer = new JsonSerializer();

            if (StreamExistsForDesiredDate(identifier.Date.Year, identifier.Date.Month, identifier.MedicalPracticeId))
            {
                var path =
                    $@"{basePath}\{identifier.MedicalPracticeId}\{identifier.Date.Year}\{identifier.Date.Month}.json";
                IEnumerable<EventStream<AggregateIdentifier>> streams;

                using (var file = File.OpenText(path))
                {
                    var streamDoubles =
                        (List<EventStreamSerializationDouble>)
                            serializer.Deserialize(file, typeof (List<EventStreamSerializationDouble>));
                    streams = streamDoubles.Select(eventStreamDouble => eventStreamDouble.GetEventStream());
                }
                return streams.FirstOrDefault(evenstream => evenstream.Id == identifier);
            }
            return new EventStream<AggregateIdentifier>(identifier);
        }

        public List<EventStream<AggregateIdentifier>> LoadInitialEventStreams()
        {
            var streams = new List<EventStream<AggregateIdentifier>>();

            foreach (var practice in config.GetAllMedicalPractices())
            {
                if (!StreamExistsForPractice(practice.Id))
                {
                    CreateInitialDirectory(practice.Id);
                }
                else
                {
                    var currentMonthDate = new Date(DateTime.Today);
                    var prevMonthDate = new Date(DateTime.Now.AddMonths(-1));
                    var nextMonthDate = new Date(DateTime.Now.AddMonths(1));

                    streams.AddRange(GetStreamsForMonth(practice.Id, prevMonthDate.Year, prevMonthDate.Month));
                    streams.AddRange(GetStreamsForMonth(practice.Id, currentMonthDate.Year, currentMonthDate.Month));
                    streams.AddRange(GetStreamsForMonth(practice.Id, nextMonthDate.Year, nextMonthDate.Month));
                }
            }
            return streams;
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

        private IList<EventStream<AggregateIdentifier>> GetStreamsForMonth(Guid id, ushort year, byte month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                .Select(
                    day =>
                        LoadEventStream(new AggregateIdentifier(new Date(Convert.ToByte(day), month, year), id,
                            config.GetLatestVersionFor(id))))
                .ToList();
        }

        private void CreateInitialDirectory(Guid id)
        {
            var path = $@"{basePath}\{id}";

            Directory.CreateDirectory(path);
        }

        private bool StreamExistsForDesiredDate(ushort year, byte month, Guid practiceId)
        {
            var path = $@"{basePath}\{practiceId}\{year}\{month}.json";

            return File.Exists(path);
        }

        private bool StreamExistsForPractice(Guid practiceId)
        {
            var path = $@"{basePath}\{practiceId}";

            return Directory.Exists(path);
        }
    }
}
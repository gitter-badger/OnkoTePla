using System;
using System.Collections.Generic;
using System.IO;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Core.Repositories.StreamManagement
{
    public class StreamMetaDataService : IStreamMetaDataService
    {
        private readonly string baseDir;
        private Dictionary<Guid, PracticeMetaData> metaDataFiles;

        public StreamMetaDataService(string baseDir)
        {
            this.baseDir = baseDir;
        }

        private PracticeMetaData ReadMetaDataForPractice(Guid practiceId)
        {
            var path = GetMetaDataPathForPractice(practiceId);

            if (File.Exists(path))
            {
                using (var file = File.OpenText(path))
                {
                    var content = file.ReadToEnd();
                    return JsonConvert.DeserializeObject<PracticeMetaData>(content);
                }
            }

            return new PracticeMetaData();
        }

        private void SaveMetaData(Guid practiceId, PracticeMetaData data)
        {
            var path = GetMetaDataPathForPractice(practiceId);

            using (var output = new StringWriter())
            {
                output.Write(JsonConvert.SerializeObject(data));
                File.WriteAllText(path, output.ToString());
            }
        }

        public void PersistMetaData()
        {
            foreach (var metaDataFile in metaDataFiles)
            {
                SaveMetaData(metaDataFile.Key, metaDataFile.Value);
            }
        }

        public void UpdateMetaDataForPractice(IEnumerable<EventStream<AggregateIdentifier>> streams)
        {
            UpdateFirstAppointmentDate(streams);
            UpdateLastAppointmentDate(streams);
            UpdateAppointmentsExistenceIndex(streams);
        }

        public void UpdateMetaDataForPatient(IEnumerable<EventStream<AggregateIdentifier>> streams, Guid patientId)
        {
            UpdateAppointmentsForPatient(streams, patientId);
        }

        private void UpdateAppointmentsExistenceIndex(IEnumerable<EventStream<AggregateIdentifier>> streams)
        {
            throw new NotImplementedException();
        }

        private void UpdateLastAppointmentDate(IEnumerable<EventStream<AggregateIdentifier>> streams)
        {
            throw new NotImplementedException();
        }

        private void UpdateFirstAppointmentDate(IEnumerable<EventStream<AggregateIdentifier>> streams)
        {
            throw new NotImplementedException();
        }

        private void UpdateAppointmentsForPatient(IEnumerable<EventStream<AggregateIdentifier>> streams, Guid patientId)
        {
            throw new NotImplementedException();
        }

        public PracticeMetaData GetMetaDataForPractice(Guid practiceId)
        {
            if (!metaDataFiles.ContainsKey(practiceId))
            {
                metaDataFiles.Add(practiceId, ReadMetaDataForPractice(practiceId));
            }

            return metaDataFiles[practiceId];
        }

        private string GetMetaDataPathForPractice(Guid practiceId)
        {
            return $@"{baseDir}\{practiceId}\metaData.json";
        }
    }
}
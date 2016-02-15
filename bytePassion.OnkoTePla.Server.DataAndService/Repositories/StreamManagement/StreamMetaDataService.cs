using System;
using System.Collections.Generic;
using System.IO;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
    public class StreamMetaDataService : IStreamMetaDataService
    {
        private readonly string baseDir;
        private readonly Dictionary<Guid, PracticeMetaData> metaDataFiles;

        public StreamMetaDataService(string baseDir)
        {
            metaDataFiles = new Dictionary<Guid, PracticeMetaData>();
            this.baseDir = baseDir;
        }

        public void PersistMetaData()
        {
            foreach (var metaDataFile in metaDataFiles)
            {
                SaveMetaData(metaDataFile.Key, metaDataFile.Value);
            }
        }

        public void UpdateMetaData(DomainEvent @event)
        {
            if (!metaDataFiles.ContainsKey(@event.AggregateId.MedicalPracticeId))
            {
                metaDataFiles.Add(@event.AggregateId.MedicalPracticeId, new PracticeMetaData());
            }
            UpdateMetaDataForPractice(@event);
            UpdateMetaDataForPatient(@event);
        }

        public PracticeMetaData GetMetaDataForPractice(Guid practiceId)
        {
            if (!metaDataFiles.ContainsKey(practiceId))
            {
                metaDataFiles.Add(practiceId, ReadMetaDataForPractice(practiceId));
            }

            return metaDataFiles[practiceId];
        }

        public void Initialize()
        {
            var directories = Directory.GetDirectories($@"{baseDir}");

                foreach (var directory in directories)
                {
                    var id = Guid.Parse(new DirectoryInfo(directory).Name);
                    metaDataFiles.Add(id, ReadMetaDataForPractice(id));
                }
        }

        private PracticeMetaData ReadMetaDataForPractice(Guid practiceId)
        {
            var path = GetMetaDataPathForPractice(practiceId);
            //var settings = new JsonSerializerSettings();
            //settings.Converters.Add(new PracticeExistenceIndexConverter());

            if (File.Exists(path))
            {
                using (var file = File.OpenText(path))
                {
                    var content = file.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<PracticeMetaData>(content);
                    return data;
                }
            }



            return new PracticeMetaData();
        }

        private void SaveMetaData(Guid practiceId, PracticeMetaData data)
        {

            Directory.CreateDirectory($@"{baseDir}\{practiceId}");

            var path = GetMetaDataPathForPractice(practiceId);
            //var settings = new JsonSerializerSettings();
            //settings.Converters.Add(new PracticeExistenceIndexConverter());

            if (!File.Exists(path))
            {
                using (var stream = File.Create(path))
                {

                }
            }

            using (var output = new StringWriter())
            {
                output.Write(JsonConvert.SerializeObject(data));
                File.WriteAllText(path, output.ToString());
            }

        }

        private void UpdateMetaDataForPractice(DomainEvent @event)
        {
            UpdateFirstAndLastAppointmentDate(@event);
            UpdateAppointmentsExistenceIndex(@event);
        }


        private void UpdateMetaDataForPatient(DomainEvent @event)
        {
            var id = @event.AggregateId.MedicalPracticeId;
            var targetDate = new DateTime(@event.AggregateId.Date.Year, @event.AggregateId.Date.Month, @event.AggregateId.Date.Day);
            var patientId = @event.PatientId;
            var patientDictionary = metaDataFiles[id].AppointmentsForPatient;

            if (!patientDictionary.ContainsKey(patientId))
                patientDictionary.Add(patientId, new List<DateTime>());

            if (@event.GetType() == typeof (AppointmentAdded))
            {
                if (!patientDictionary[patientId].Contains(targetDate))
                {
                    patientDictionary[patientId].Add(targetDate);
                }
            }
            else if (@event.GetType() == typeof (AppointmentDeleted))
            {
                if (patientDictionary[patientId].Contains(targetDate))
                {
                    patientDictionary[patientId].Remove(targetDate);
                }
            }
        }

        private void UpdateAppointmentsExistenceIndex(DomainEvent @event)
        {
            var id = @event.AggregateId.MedicalPracticeId;
            var targetDate = new DateTime(@event.AggregateId.Date.Year, @event.AggregateId.Date.Month, @event.AggregateId.Date.Day);

            if (@event.GetType() == typeof (AppointmentAdded))
            {
                if (!metaDataFiles[id].AppointmentExistenceIndex.ContainsKey(targetDate))
                {
                    metaDataFiles[id].AppointmentExistenceIndex.Add(targetDate, 1);
                }
                else
                {
                    metaDataFiles[id].AppointmentExistenceIndex[targetDate] += 1;
                }
            }
            else if (@event.GetType() == typeof (AppointmentDeleted))
            {
                if (!metaDataFiles[id].AppointmentExistenceIndex.ContainsKey(targetDate))
                {
                    metaDataFiles[id].AppointmentExistenceIndex.Add(targetDate, 0);
                }
                else
                {
                    metaDataFiles[id].AppointmentExistenceIndex[targetDate] -= 1;
                }
            }
        }

        private void UpdateFirstAndLastAppointmentDate(DomainEvent @event)
        {
            var targetDate = new DateTime(@event.AggregateId.Date.Year, @event.AggregateId.Date.Month, @event.AggregateId.Date.Day);

            if (metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate >
                     targetDate || (metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate == DateTime.MinValue))
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate =
                    targetDate;
            }
           if (metaDataFiles[@event.AggregateId.MedicalPracticeId].LastAppointmentDate <
                     targetDate)
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].LastAppointmentDate =
                    targetDate;
            }
        }

        private string GetMetaDataPathForPractice(Guid practiceId)
        {
            return $@"{baseDir}\{practiceId}\metaData.json";
        }
    }
}
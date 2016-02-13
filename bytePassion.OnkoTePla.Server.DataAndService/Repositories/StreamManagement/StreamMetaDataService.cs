using System;
using System.Collections.Generic;
using System.IO;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using Newtonsoft.Json;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamManagement
{
    public class StreamMetaDataService : IStreamMetaDataService
    {
        private readonly string baseDir;
        private readonly Dictionary<Guid, PracticeMetaData> metaDataFiles;
        private readonly List<Guid> loadedPractices = new List<Guid>();

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
            var id = @event.AggregateId.MedicalPracticeId;

            if (!loadedPractices.Contains(id))
            {
                GetMetaDataForPractice(id);
                loadedPractices.Add(id);
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

            Directory.CreateDirectory($@"{baseDir}\{practiceId}");
            File.Create(path);

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

        private void UpdateMetaDataForPractice(DomainEvent @event)
        {
            UpdateFirstAndLastAppointmentDate(@event);
            UpdateAppointmentsExistenceIndex(@event);
        }


        private void UpdateMetaDataForPatient(DomainEvent @event)
        {
            var id = @event.AggregateId.MedicalPracticeId;
            var targetDate = @event.AggregateId.Date;
            var patientId = @event.PatientId;
            var patientDictionary = metaDataFiles[id].AppointmentsForPatient;

            if (!patientDictionary.ContainsKey(patientId))
                patientDictionary.Add(patientId, new List<Date>());

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
            var targetDate = @event.AggregateId.Date;

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
            var firstDate = metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate;
            var lastDate = metaDataFiles[@event.AggregateId.MedicalPracticeId].LastAppointmentDate;

            if (ReferenceEquals(null, firstDate))
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate =
                    @event.AggregateId.Date;
            }
            else if (metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate >
                     @event.AggregateId.Date)
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate =
                    @event.AggregateId.Date;
            }
            if (ReferenceEquals(null, lastDate))
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].LastAppointmentDate =
                    @event.AggregateId.Date;
            } 
            else if (metaDataFiles[@event.AggregateId.MedicalPracticeId].FirstAppointmentDate <
                     @event.AggregateId.Date)
            {
                metaDataFiles[@event.AggregateId.MedicalPracticeId].LastAppointmentDate =
                    @event.AggregateId.Date;
            }
        }

        private string GetMetaDataPathForPractice(Guid practiceId)
        {
            return $@"{baseDir}\{practiceId}\metaData.json";
        }
    }
}
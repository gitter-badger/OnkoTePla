using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData
{
	public class StreamMetaDataService : IStreamMetaDataService
    {
		private readonly IPersistenceService<IEnumerable<IPracticeMetaData>> persistenceService;
		private Dictionary<Guid, IPracticeMetaData> metaDataFiles;

        public StreamMetaDataService(IPersistenceService<IEnumerable<IPracticeMetaData>> persistenceService)
        {
	        this.persistenceService = persistenceService;
	        metaDataFiles = new Dictionary<Guid, IPracticeMetaData>();
        }

		public void PersistRepository ()
		{
			persistenceService.Persist(metaDataFiles.Values);
		}

		public void LoadRepository ()
		{
			metaDataFiles = persistenceService.Load()
											  .ToDictionary(metaData => metaData.MedicalPracticeId,
															metaData => metaData);
		}

		public void UpdateMetaData(DomainEvent @event)
		{
			var medicalPracticeId = @event.AggregateId.MedicalPracticeId;

			if (!metaDataFiles.ContainsKey(medicalPracticeId))
			{
				metaDataFiles.Add(medicalPracticeId, new PracticeMetaData(medicalPracticeId));
			}
			
			metaDataFiles[medicalPracticeId].AddEventToMetaData(@event);			
		}

		public IEnumerable<AggregateIdentifier> GetDaysForPatient(Guid patientId)
		{
			return metaDataFiles.Values.SelectMany(
				metaData =>
				{
					if (metaData.AppointmentsForPatient.ContainsKey(patientId))
						return metaData.AppointmentsForPatient[patientId].Select(date => new AggregateIdentifier(date, metaData.MedicalPracticeId));
					else
						return new List<AggregateIdentifier>();
				}
			);			
		}		
    }
}
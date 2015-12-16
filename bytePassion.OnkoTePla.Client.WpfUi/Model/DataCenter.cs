using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Model
{

	public class DataCenter : IDataCenter
	{
		public DataCenter(IConfigurationReadRepository configuration, 
						  IPatientReadRepository patients, 
						  IReadModelRepository readModelRepository, 
						  SessionInformation sessionInfo)
		{
			Configuration = configuration;
			Patients = patients;
			ReadModelRepository = readModelRepository;
			SessionInfo = sessionInfo;

			dataCache = new Dictionary<Guid, IDictionary<Date, MedicalPractice>>();
		}

		public IConfigurationReadRepository Configuration       { get; }
		public IPatientReadRepository       Patients            { get; }
		public IReadModelRepository         ReadModelRepository { get; }
		public SessionInformation           SessionInfo         { get; }


		private readonly IDictionary<Guid, IDictionary<Date, MedicalPractice>> dataCache;

		public MedicalPractice GetMedicalPracticeByDateAndId(Date date, Guid medicalPracticeId)
		{			
			if (!dataCache.ContainsKey(medicalPracticeId))
				dataCache.Add(medicalPracticeId, new Dictionary<Date, MedicalPractice>());

			var innerCache = dataCache[medicalPracticeId];

			if (!innerCache.ContainsKey(date))
			{
				var readModel = ReadModelRepository.GetAppointmentsOfADayReadModel(new AggregateIdentifier(date, medicalPracticeId));
				var medicalPractice = Configuration.GetMedicalPracticeByIdAndVersion(medicalPracticeId, readModel.Identifier.PracticeVersion);

				innerCache.Add(date, medicalPractice);

				readModel.Dispose();
			}
			
            return innerCache[date];
		}
	}
}

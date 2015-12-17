using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Workflow;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.WpfUi.Model
{

    internal class DataCenter : IDataCenter
	{
		public DataCenter(IConfigurationReadRepository configuration, 
						  IPatientReadRepository patients, 
						  IReadModelRepository readModelRepository, 
						  SessionInformation sessionInfo, 
                          IClientWorkflow workflow)
		{
			Configuration = configuration;
			Patients = patients;
			ReadModelRepository = readModelRepository;
			SessionInfo = sessionInfo;
		    Workflow = workflow;

		    dataCache = new Dictionary<Guid, IDictionary<Date, MedicalPractice>>();
		}

		public IConfigurationReadRepository Configuration       { get; }
		public IPatientReadRepository       Patients            { get; }
		public IReadModelRepository         ReadModelRepository { get; }
		public SessionInformation           SessionInfo         { get; }
        public IClientWorkflow              Workflow            { get; }


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

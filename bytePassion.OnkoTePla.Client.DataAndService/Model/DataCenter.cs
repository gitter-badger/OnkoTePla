using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Readmodels;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Client.DataAndService.Model
{

    public class DataCenter : IDataCenter
	{
        private readonly IConfigurationReadRepository configuration;
        private readonly IPatientReadRepository patientRepository;
        private readonly IReadModelRepository readModelRepository;
        private readonly SessionInformation sessionInfo;

        public DataCenter(IConfigurationReadRepository configuration, 
						  IPatientReadRepository patientRepository, 
						  IReadModelRepository readModelRepository, 
						  SessionInformation sessionInfo, 
                          IClientWorkflow workflow)
        {
            this.configuration = configuration;
            this.patientRepository = patientRepository;
            this.readModelRepository = readModelRepository;
            this.sessionInfo = sessionInfo;

            dataCache = new Dictionary<Guid, IDictionary<Date, MedicalPractice>>();
        }


        private readonly IDictionary<Guid, IDictionary<Date, MedicalPractice>> dataCache;

        public User LoggedInUser => sessionInfo.LoggedInUser;

        public AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier identifier)
        {
            return readModelRepository.GetAppointmentsOfADayReadModel(identifier);
        }

        public AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid patientId)
        {
            return readModelRepository.GetAppointmentsOfAPatientReadModel(patientId);
        }

        public MedicalPractice GetMedicalPracticeByDateAndId(Date date, Guid medicalPracticeId)
		{			
			if (!dataCache.ContainsKey(medicalPracticeId))
				dataCache.Add(medicalPracticeId, new Dictionary<Date, MedicalPractice>());

			var innerCache = dataCache[medicalPracticeId];

			if (!innerCache.ContainsKey(date))
			{
				var readModel = readModelRepository.GetAppointmentsOfADayReadModel(new AggregateIdentifier(date, medicalPracticeId));
				var medicalPractice = configuration.GetMedicalPracticeByIdAndVersion(medicalPracticeId, readModel.Identifier.PracticeVersion);

				innerCache.Add(date, medicalPractice);

				readModel.Dispose();
			}
			
            return innerCache[date];
		}

        public MedicalPractice GetMedicalPracticeByIdAndVersion(Guid medicalPracticeId, uint version = 0)
        {
            return configuration.GetMedicalPracticeByIdAndVersion(medicalPracticeId, version);
        }

        public IEnumerable<MedicalPractice> GetAllMedicalPractices()
        {
            return configuration.GetAllMedicalPractices();
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return patientRepository.GetAllPatients();
        }
	}
}

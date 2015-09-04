using System;
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
		}

		public IConfigurationReadRepository Configuration       { get; }
		public IPatientReadRepository       Patients            { get; }
		public IReadModelRepository         ReadModelRepository { get; }
		public SessionInformation           SessionInfo         { get; }


		public MedicalPractice GetMedicalPracticeByDateAndId(Date date, Guid medicalPracticeId)
		{
			// TODO: speed up by caching

			var readModel = ReadModelRepository.GetAppointmentsOfADayReadModel(new AggregateIdentifier(date, medicalPracticeId));
			return Configuration.GetMedicalPracticeByIdAndVersion(medicalPracticeId, readModel.Identifier.PracticeVersion);
		}
	}
}

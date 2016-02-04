using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Readmodels;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Core.Repositories.Readmodel
{
	public class ReadModelRepository : IReadModelRepository
	{
		private readonly IClientEventBus eventBus;
		private readonly IEventStore eventStore;
		private readonly IConfigurationReadRepository config;
		private readonly IPatientReadRepository patientsRepository;

		public ReadModelRepository (IClientEventBus eventBus, IEventStore eventStore,
 								   IPatientReadRepository patientsRepository,
								   IConfigurationReadRepository config)
		{
			this.eventStore = eventStore;
			this.config = config;
			this.patientsRepository = patientsRepository;
			this.eventBus = eventBus;
		}

		public FixedAppointmentSet GetAppointmentSetOfADay (AggregateIdentifier id, uint? eventStreamVersionLimit)
		{
			var eventStream = eventStore.GetEventStreamForADay(id);
			var medicalPractice = new ClientMedicalPracticeData(config.GetMedicalPracticeByIdAndVersion(id.MedicalPracticeId, 
																										id.PracticeVersion));
			return new FixedAppointmentSet(medicalPractice, patientsRepository, 
										   eventStream, eventStreamVersionLimit);
		}

		public AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier id)
		{
			var eventStream = eventStore.GetEventStreamForADay(id);
			var medicalPractice = new ClientMedicalPracticeData(config.GetMedicalPracticeByIdAndVersion(id.MedicalPracticeId,
																										id.PracticeVersion));
			var readmodel = new AppointmentsOfADayReadModel(eventBus, medicalPractice, 
															patientsRepository, eventStream.Id);
			readmodel.LoadFromEventStream(eventStream);

			return readmodel;
		}

		public AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid patientId)
		{
			Func<Guid, uint, ClientMedicalPracticeData> practiceRepository = 
				(id, version) => new ClientMedicalPracticeData(config.GetMedicalPracticeByIdAndVersion(id,version));

			var readModel = new AppointmentsOfAPatientReadModel(patientId, eventBus, practiceRepository, patientsRepository);
			readModel.LoadFromEventStream(eventStore.GetEventStreamForAPatient(patientId));

			return readModel;
		}
	}
}

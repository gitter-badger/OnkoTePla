
using System;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel
{
	public class ReadModelRepository : IReadModelRepository
	{
		private readonly IEventBus eventBus;
		private readonly IEventStore eventStore;
		private readonly IConfigurationReadRepository config;
		private readonly IPatientReadRepository patientsRepository;

		public ReadModelRepository (IEventBus eventBus, IEventStore eventStore,
 								   IPatientReadRepository patientsRepository,
								   IConfigurationReadRepository config)
		{
			this.eventStore = eventStore;
			this.config = config;
			this.patientsRepository = patientsRepository;
			this.eventBus = eventBus;
		}

		public AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier id)
		{
			var eventStream = eventStore.GetEventStream(id);
			var readmodel = new AppointmentsOfADayReadModel(eventBus, config, patientsRepository, eventStream.Id);
			readmodel.LoadFromEventStream(eventStream);

			return readmodel;
		}

		public AppointmentsOfAPatientReadModel GetAppointmentsOfAPatientReadModel(Guid patientId)
		{
			var readModel = new AppointmentsOfAPatientReadModel(patientId, eventBus, config, patientsRepository);
			readModel.LoadFromEventStream(eventStore.GetEventStreamForAPatient(patientId));

			return readModel;
		}
	}
}


using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel
{
	public class ReadModelRepository : IReadModelRepository
	{
		private readonly IEventBus eventBus;
		private readonly IEventStore eventstore;
		private readonly IConfigurationReadRepository config;
		private readonly IPatientReadRepository patientsRepository;

		public ReadModelRepository(IEventBus eventBus, IEventStore eventstore,
 								   IPatientReadRepository patientsRepository,
								   IConfigurationReadRepository config)
		{
			this.eventstore = eventstore;
			this.config = config;
			this.patientsRepository = patientsRepository;
			this.eventBus = eventBus;
		}

		public AppointmentsOfADayReadModel GetAppointmentsOfADayReadModel(AggregateIdentifier id)
		{
			var eventStream = eventstore.GetEventStream(id);
			var readmodel = new AppointmentsOfADayReadModel(eventBus, config, patientsRepository, eventStream.Id);
			readmodel.LoadFromEventStream(eventStream);

			return readmodel;
		}
	}
}

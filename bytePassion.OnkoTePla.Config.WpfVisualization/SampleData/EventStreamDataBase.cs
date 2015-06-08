
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
	public static class EventStreamDataBase
	{


		public static void GenerateExampleEventStream(Configuration config)
		{			
			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new XmlPatientDataStore("patients.xml");
			IPatientReadRepository patientRepository = new PatientRepository(patientPersistenceService);

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore("config.xml");
			IConfigurationRepository configRepository = new ConfigurationRepository(configPersistenceService);

			IPersistenceService<IEnumerable<EventStream>> eventStorePersistenceService = new XmlEventStreamDataStore("eventHistory.xml");
			IEventStore eventStore = new EventStore(eventStorePersistenceService, configRepository);

			IEventBus eventBus = new EventBus();
			ICommandBus commandBus = new CommandBus();

			IAggregateRepository repository = new AggregateRepository(eventBus, eventStore, patientRepository, configRepository);

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(repository));

			

			var medicalPratice = configRepository.GetMedicalPracticeByName("examplePractice1");
			var patient = patientRepository.GetAllPatients().First();

			// aggregate version fehlt noch .. aus readmodel
			//commandBus.Send(new AddAppointment(new AggregateIdentifier(new Date(8,6,2015), medicalPratice.Id), ));



		} 
	}
}

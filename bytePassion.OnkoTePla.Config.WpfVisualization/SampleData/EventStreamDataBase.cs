
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
	public static class EventStreamDataBase
	{


		public static void GenerateExampleEventStream()
		{			
			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new XmlPatientDataStore("patients.xml");
			IPatientReadRepository patientRepository = new PatientRepository(patientPersistenceService);
			patientRepository.LoadRepository();

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore("config.xml");
			IConfigurationRepository configRepository = new ConfigurationRepository(configPersistenceService);
			configRepository.LoadRepository();

			IPersistenceService<IEnumerable<EventStream>> eventStorePersistenceService = new XmlEventStreamDataStore("eventHistory.xml");
			IEventStore eventStore = new EventStore(eventStorePersistenceService, configRepository);

			IEventBus eventBus = new EventBus();
			ICommandBus commandBus = new CommandBus();

			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientRepository, configRepository);
			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientRepository, configRepository);

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));

			var medicalPratice = configRepository.GetMedicalPracticeByName("examplePractice1");
			var aggregateID = new AggregateIdentifier(new Date(8, 6, 2015), medicalPratice.Id);						
			var readmodel = readModelRepository.GetAppointmentsOfADayReadModel(aggregateID);
			var patient = patientRepository.GetAllPatients().First();
			var user = configRepository.GetUserByName("exampleUser1");
			var room = medicalPratice.Rooms.First();
			var therapyPlace = room.TherapyPlaces.First();

			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion, 
											   user.Id, patient.Id, 
											   "first Appointment through cqrs system", 
											   new Time(10,0), new Time(12,00), 
											   therapyPlace.Id, room.Id));



			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion,
											   user.Id, patient.Id,
											   "first Appointment through cqrs system",
											   new Time(13, 0), new Time(15, 00),
											   therapyPlace.Id, room.Id));

			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion,
											   user.Id, patient.Id,
											   "first Appointment through cqrs system",
											   new Time(16, 0), new Time(18, 00),
											   therapyPlace.Id, room.Id));


			eventStore.PersistRepository();

			var readmodel2 = readModelRepository.GetAppointmentsOfADayReadModel(aggregateID);
		} 
	}
}

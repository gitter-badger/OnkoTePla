
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
	public static class EventStreamDataBase
	{


		public static void GenerateExampleEventStream()
		{			
			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new XmlPatientDataStore(GlobalConstants.PatientPersistenceFile);
			IPatientReadRepository patientRepository = new PatientRepository(patientPersistenceService);
			patientRepository.LoadRepository();

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			IConfigurationReadRepository configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();

			IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> eventStorePersistenceService = new XmlEventStreamDataStore(GlobalConstants.EventHistoryPersistenceFile);
			IEventStore eventStore = new EventStore(eventStorePersistenceService, configReadRepository);

			IEventBus eventBus = new EventBus();
			ICommandBus commandBus = new CommandBus();

			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientRepository, configReadRepository);
			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientRepository, configReadRepository);

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));


			//////////////////////////////

			var medicalPratice = configReadRepository.GetMedicalPracticeByName("examplePractice1");
			var aggregateID = new AggregateIdentifier(new Date(8, 6, 2015), medicalPratice.Id);						
			var readmodel = readModelRepository.GetAppointmentsOfADayReadModel(aggregateID);
			var patient = patientRepository.GetAllPatients().First();
			var user = configReadRepository.GetUserByName("exampleUser1");
			var room = medicalPratice.Rooms.First();
			var therapyPlace = room.TherapyPlaces.First();

			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion, 
											   user.Id, patient.Id, 
											   "first Appointment through cqrs system", 
											   new Time(10,0), new Time(12,00), 
											   therapyPlace.Id));



			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion,
											   user.Id, patient.Id,
											   "first Appointment through cqrs system",
											   new Time(13, 0), new Time(15, 00),
											   therapyPlace.Id));

			commandBus.Send(new AddAppointment(aggregateID, readmodel.AggregateVersion,
											   user.Id, patient.Id,
											   "first Appointment through cqrs system",
											   new Time(16, 0), new Time(18, 00),
											   therapyPlace.Id));


			eventStore.PersistRepository();

			var readmodel2 = readModelRepository.GetAppointmentsOfADayReadModel(aggregateID);
		} 
	}
}

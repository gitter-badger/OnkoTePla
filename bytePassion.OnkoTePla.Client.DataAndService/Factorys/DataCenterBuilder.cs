using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.DataAndService.LocalSettings;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Resources;

namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
{
	public class DataCenterBuilder : IDataCenterBuilder
	{
		public IDataCenter Build()
		{
			// Patient-Repository

			var patientPersistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
			var patientReadRepository = new PatientRepository(patientPersistenceService);
			patientReadRepository.LoadRepository();


			// Config-Repository

			var configPersistenceService = new JsonConfigurationDataStore(GlobalConstants.ConfigJsonPersistenceFile);
			var configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();


			// LocalSettings-Repository

			var settingPersistenceService = new LocalSettingsXMLPersistenceService(GlobalConstants.LocalSettingsPersistenceFile);
			var localSettingsRepository = new LocalSettingsRepository(settingPersistenceService);
			localSettingsRepository.LoadRepository();


			// EventStore

			var eventStorePersistenceService = new JsonEventStreamDataStore(GlobalConstants.EventHistoryJsonPersistenceFile);
			var eventStore = new EventStore(eventStorePersistenceService, configReadRepository);
			eventStore.LoadRepository();


			// Event- and CommandBus

			var eventHandlerCollection = new MultiHandlerCollection <DomainEvent>();
			var eventMessageBus        = new LocalMessageBus<DomainEvent>(eventHandlerCollection);
			var eventBus               = new EventBus(eventMessageBus);
			
			var commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
			var commandMessageBus        = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
			var commandBus               = new CommandBus(commandMessageBus);


			// Aggregate- and Readmodel-Repositories

			var aggregateRepository = new AggregateRepository(eventBus, eventStore, patientReadRepository, configReadRepository);
			var readModelRepository = new ReadModelRepository(eventBus, eventStore, patientReadRepository, configReadRepository);


			// Register CommandHandler

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));
			commandBus.RegisterCommandHandler(new DeleteAppointmentCommandHandler(aggregateRepository));
			commandBus.RegisterCommandHandler(new ReplaceAppointmentCommandHandler(aggregateRepository));
			
			return new DataCenter(configReadRepository,
								  patientReadRepository,
								  readModelRepository,
								  localSettingsRepository,
								  commandBus,
								  eventStore);
		}
	}
}

//namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
//{
//	public class DataCenterBuilder : IDataCenterBuilder
//	{
//		public IDataCenter Build()
//		{
//			// Patient-Repository
//
//			var patientPersistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
//			var patientReadRepository = new PatientRepository(patientPersistenceService);
//			patientReadRepository.LoadRepository();
//
//
//			// Config-Repository
//
//			var configPersistenceService = new JsonConfigurationDataStore(GlobalConstants.ConfigJsonPersistenceFile);
//			var configReadRepository = new ConfigurationRepository(configPersistenceService);
//			configReadRepository.LoadRepository();
//
//
//			// LocalSettings-Repository
//
//			var settingPersistenceService = new LocalSettingsXMLPersistenceService(GlobalConstants.LocalSettingsPersistenceFile);
//			var localSettingsRepository = new LocalSettingsRepository(settingPersistenceService);
//			localSettingsRepository.LoadRepository();
//
//
//            // EventStore
//
//            var streamPersistenceService = new StreamPersistenceService(configReadRepository, GlobalConstants.EventHistoryBasePath);
//            var streamManager = new StreamManagementService(streamPersistenceService);
//            var eventStorePersistenceService = new JsonEventStreamDataStore(GlobalConstants.EventHistoryJsonPersistenceFile);
//			var eventStore = new EventStore(eventStorePersistenceService, streamManager, configReadRepository);
//			eventStore.LoadRepository();
//
//            
//
//			// Event- and CommandBus
//
//			var eventHandlerCollection = new MultiHandlerCollection <DomainEvent>();
//			var eventMessageBus        = new LocalMessageBus<DomainEvent>(eventHandlerCollection);
//			var eventBus               = new EventBus(eventMessageBus);
//			
//			var commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
//			var commandMessageBus        = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
//			var commandBus               = new CommandBus(commandMessageBus);
//
//
//			// Aggregate- and Readmodel-Repositories
//
//			var aggregateRepository = new AggregateRepository(eventBus, eventStore, patientReadRepository, configReadRepository);
//			var readModelRepository = new ReadModelRepository(null, eventStore, patientReadRepository, configReadRepository);
//			
//
//			// Register CommandHandler
//
//			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));
//			commandBus.RegisterCommandHandler(new DeleteAppointmentCommandHandler(aggregateRepository));
//			commandBus.RegisterCommandHandler(new ReplaceAppointmentCommandHandler(aggregateRepository));
//			
//			return new DataCenter(configReadRepository,
//								  patientReadRepository,
//								  readModelRepository,
//								  localSettingsRepository,
//								  commandBus,
//								  eventStore);
//
//			return null;
//		}
//	}
//}

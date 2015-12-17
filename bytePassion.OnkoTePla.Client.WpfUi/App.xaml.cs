using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;
using bytePassion.OnkoTePla.Client.WpfUi.Model;
using bytePassion.OnkoTePla.Client.WpfUi.SessionInfo;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Readmodels;
using bytePassion.OnkoTePla.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using System.Linq;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi
{

    public partial class App
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////                          Composition Root and Setup                         //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////


			// Patient-Repository

			var patientPersistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
			var patientReadRepository = new PatientRepository(patientPersistenceService);
			patientReadRepository.LoadRepository();


			// Config-Repository

			var configPersistenceService = new JsonConfigurationDataStore(GlobalConstants.ConfigJsonPersistenceFile);
			var configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();


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


			// SessionInformation

			var sessionInformation = new SessionInformation
			{
				LoggedInUser = configReadRepository.GetAllUsers().First()
			};


			// SessionAndUserSpecificEventHistory

			var sessionAndUserSpecificEventHistory = new SessionAndUserSpecificEventHistory(eventBus,
																							commandBus,
																							readModelRepository,
                                                                                            patientReadRepository,
                                                                                            configReadRepository,
																							sessionInformation.LoggedInUser,
																							50);

			// Data-Model

			var dataCenter = new DataCenter(configReadRepository, 
											patientReadRepository, 
											readModelRepository, 
											sessionInformation);

            // initiate ViewModelCommunication			

            IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
            IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
            IViewModelCollections viewModelCollections = new ViewModelCollections();

            IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
                                                                                        viewModelCollections);


            // Create MainWindow

            var mainWindowBuilder = new MainWindowBuilder(dataCenter, 
                                                          viewModelCommunication,
														  commandBus, 
														  sessionAndUserSpecificEventHistory);

			var mainWindow = mainWindowBuilder.BuildWindow();
			mainWindow.ShowDialog();


			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////             Clean Up and store data after main Window was closed            //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////


			eventStore.PersistRepository();
		}		
	}
}

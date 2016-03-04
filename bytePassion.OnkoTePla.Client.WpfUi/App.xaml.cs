using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActionFactory;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.LocalSettings;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.TherapyPlaceTypeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;
using bytePassion.OnkoTePla.Resources;


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
						
			var connectionService = new ConnectionService();			
			var eventBus          = new ClientEventBus(connectionService);

			var commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
			var commandMessageBus = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
			var commandBus = new CommandBus(commandMessageBus);			

			var persistenceService = new LocalSettingsXMLPersistenceService(GlobalConstants.LocalSettingsPersistenceFile);
			var localSettingsRepository = new LocalSettingsRepository(persistenceService);
			
			var clientMedicalPracticeRepository  = new ClientMedicalPracticeRepository(connectionService);
			var clientPatientRepository          = new ClientPatientRepository(connectionService);
			var clienttherapyPlaceTypeRepository = new ClientTherapyPlaceTypeRepository(connectionService);
			var clientReadmodelRepository        = new ClientReadModelRepository(eventBus, clientPatientRepository,clientMedicalPracticeRepository, connectionService);


			var workFlow = new ClientWorkflow();
			var session  = new Session(connectionService, workFlow);

			var commandService = new CommandService(session, clientReadmodelRepository, commandBus);


			var userActionBuilder = new UserActionBuilder(commandService);

			// CommandHandler

			var     addAppointmentCommandHandler = new     AddAppointmentCommandHandler(connectionService, session, clientPatientRepository,                                  userActionBuilder, CommandHandlerErrorHandler);
			var  deleteAppointmentCommandHandler = new  DeleteAppointmentCommandHandler(connectionService, session, clientPatientRepository,                                  userActionBuilder, CommandHandlerErrorHandler);
			var replaceAppointmentCommandHandler = new ReplaceAppointmentCommandHandler(connectionService, session, clientPatientRepository, clientMedicalPracticeRepository, userActionBuilder, CommandHandlerErrorHandler);

			commandBus.RegisterCommandHandler(    addAppointmentCommandHandler);
			commandBus.RegisterCommandHandler( deleteAppointmentCommandHandler);
			commandBus.RegisterCommandHandler(replaceAppointmentCommandHandler);


			// initiate ViewModelCommunication			

			var handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
            IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
            IViewModelCollectionList viewModelCollections = new ViewModelCollectionList();

            IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
                                                                                        viewModelCollections);			
          

            var mainWindowBuilder = new MainWindowBuilder(localSettingsRepository,
														  clientPatientRepository,
														  clientMedicalPracticeRepository,														  
														  clientReadmodelRepository,														  
														  commandService,
                                                          viewModelCommunication,
														  session,														   														 
                                                          "0.1.0.0");               // TODO: get real versionNumber       

			var mainWindow = mainWindowBuilder.BuildWindow(
				errorMsg =>
				{
					Current.Dispatcher.Invoke(() =>
					{
						MessageBox.Show("fatal Error >>>>");
					});
				}					
			);

			mainWindow.ShowDialog();



			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////             Clean Up and store data after main Window was closed            //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////

			connectionService.Dispose();
		}

		private void CommandHandlerErrorHandler(string errorMessage)
		{
			MessageBox.Show($"CommandHandler failed: {errorMessage}");
		}
	}
}

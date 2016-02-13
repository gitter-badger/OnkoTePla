using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus;
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
			var workFlow          = new ClientWorkflow();			
			var session           = new Session(connectionService, workFlow);
			var eventBus          = new ClientEventBus();

			var commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
			var commandMessageBus = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
			var commandBus = new CommandBus(commandMessageBus);

			var persistenceService = new LocalSettingsXMLPersistenceService(GlobalConstants.LocalSettingsPersistenceFile);
			var localSettingsRepository = new LocalSettingsRepository(persistenceService);
			
			var clientMedicalPracticeRepository  = new ClientMedicalPracticeRepository(connectionService);
			var clientPatientRepository          = new ClientPatientRepository(connectionService);
			var clienttherapyPlaceTypeRepository = new ClientTherapyPlaceTypeRepository(connectionService);
			var clientReadmodelRepository        = new ClientReadModelRepository(eventBus, clientPatientRepository,clientMedicalPracticeRepository, connectionService);
			

			// initiate ViewModelCommunication			

			var handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
            IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
            IViewModelCollections viewModelCollections = new ViewModelCollections();

            IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
                                                                                        viewModelCollections);			
          

            var mainWindowBuilder = new MainWindowBuilder(localSettingsRepository,clientPatientRepository,clientMedicalPracticeRepository,clienttherapyPlaceTypeRepository,clientReadmodelRepository,commandBus, 
                                                          viewModelCommunication,
														  session,														   														 
                                                          "0.1.0.0");               // TODO: get real versionNumber       

			var mainWindow = mainWindowBuilder.BuildWindow(
				errorMsg =>
				{
					Application.Current.Dispatcher.Invoke(() =>
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
	}
}

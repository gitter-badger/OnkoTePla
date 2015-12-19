using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Factorys;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;


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
		

			// create session

			var connectionService = new ConnectionService();
			var workFlow = new ClientWorkflow();

			var session = new Session(connectionService, workFlow);
		

			// Data-Center

			var dataCenter = new DataCenterBuilder().Build();


            // initiate ViewModelCommunication			

            IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
            IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
            IViewModelCollections viewModelCollections = new ViewModelCollections();

            IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
                                                                                        viewModelCollections);			

            // Create MainWindow

            var mainWindowBuilder = new MainWindowBuilder(dataCenter, 
                                                          viewModelCommunication,
														  session,														   														 
                                                          "0.1.0.0");               // TODO: get real versionNumber       

			var mainWindow = mainWindowBuilder.BuildWindow();
			mainWindow.ShowDialog();


			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////             Clean Up and store data after main Window was closed            //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////

			dataCenter.PersistEventstore();
		}		
	}
}

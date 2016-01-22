using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.Factorys;
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
		
			var sessionBuilder = new SessionBuilder();
			
			var session    = sessionBuilder.Build();					
			var dataCenter = new DataCenterBuilder().Build();


            // initiate ViewModelCommunication			

            IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
            IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
            IViewModelCollections viewModelCollections = new ViewModelCollections();

            IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
                                                                                        viewModelCollections);			
          

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
			

			sessionBuilder.DisposeSession();

			dataCenter.PersistEventstore();					// TODO: just for testing
			dataCenter.PersistLocalSettings();
		}		
	}
}

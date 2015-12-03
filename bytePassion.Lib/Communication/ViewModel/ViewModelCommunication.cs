using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.FrameworkExtensions;
using System.Linq;


namespace bytePassion.Lib.Communication.ViewModel
{

    public class ViewModelCommunication : IViewModelCommunication
	{
		private readonly IMessageBus<ViewModelMessage> viewModelMessageBus;
		private readonly IViewModelCollections     viewModelCollections;				

		public ViewModelCommunication(IMessageBus<ViewModelMessage> viewModelMessageBus, 								
									  IViewModelCollections viewModelCollections)
		{
			this.viewModelMessageBus = viewModelMessageBus;			
			this.viewModelCollections = viewModelCollections;			
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               viewModelCollections                                ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public void CreateViewModelCollection<TViewModel, TIdent>(string identifier)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			viewModelCollections.CreateViewModelCollection<TViewModel, TIdent>(identifier);
		}

		public void RemoveViewModelCollection(string identifier)
		{
			viewModelCollections.RemoveViewModelCollection(identifier);
		}

		public void RegisterViewModelAtCollection<TViewModel, TIdent>(string collectionIdentifier, 
																	  TViewModel viewModel)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TIdent>(collectionIdentifier);
			viewModelCollection.AddViewModel(viewModel);
		}

		public void DeregisterViewModelAtCollection<TViewModel, TIdent>(string collectionIdentifier, TViewModel viewModel)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TIdent>(collectionIdentifier);
			viewModelCollection.RemoveViewModel(viewModel);
		}

		public void SendTo<TIdent, TMessage>(string viewModelCollectionIdentifier, 
											 TIdent viewModelIdentifier, 
											 TMessage message)
			where TMessage : ViewModelMessage
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TIdent>(viewModelCollectionIdentifier);

			var viewModel = viewModelCollection.GetViewModel(viewModelIdentifier);

			var viewModelAsMessageHandler = viewModel as IViewModelMessageHandler<TMessage>;
			viewModelAsMessageHandler?.Process(message);
		}

	    public void SendToCollection<TIdent, TMessage>(string viewModelCollectionIdentifier, TMessage message) 
            where TMessage : ViewModelMessage
	    {
	        var viewModelCollection = viewModelCollections.GetViewModelCollection<TIdent>(viewModelCollectionIdentifier);

	        viewModelCollection.GetAllViewModelsFromCollection()
                               .Select(viewModel => viewModel as IViewModelMessageHandler<TMessage>)
                               .Do(viewModelAsMessageHandler => viewModelAsMessageHandler?.Process(message));
	    }
	   

		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               viewModelMessageBus                                 ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		public void RegisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage: ViewModelMessage
		{
			viewModelMessageBus.RegisterMessageHandler(viewModelMessageHandler);
		}

		public void DeregisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage : ViewModelMessage
		{
			viewModelMessageBus.DeregisterMessageHander(viewModelMessageHandler);
		}

		public void Send<TMessage>(TMessage message) 
			where TMessage : ViewModelMessage
		{
			viewModelMessageBus.Send(message);
		}
		
	}
}

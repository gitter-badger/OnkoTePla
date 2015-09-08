using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.Lib.Communication.ViewModel
{

	public interface IViewModelCommunication {


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               global variables                                    ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		void RegisterGlobalViewModelVariable<TVariableType>(string identifier, 
		                                                    TVariableType initialValue = default(TVariableType));

		IGlobalState<TVariableType> GetGlobalViewModelVariable<TVariableType>(string identifier);



		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               viewModelCollections                                ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////


		void CreateViewModelCollection<TViewModel, TIdent>(string identifier)
			where TViewModel : IViewModelCollectionItem<TIdent>;

		void RemoveViewModelCollection(string identifier);

		void RegisterViewModelAtCollection<TViewModel, TIdent>(string collectionIdentifier, 
		                                                       TViewModel viewModel)
			where TViewModel : IViewModelCollectionItem<TIdent>;

		void DeregisterViewModelAtCollection<TViewModel, TIdent>(string collectionIdentifier, TViewModel viewModel)
			where TViewModel : IViewModelCollectionItem<TIdent>;

		void SendTo<TIdent, TMessage>(string viewModelCollectionIdentifier, 
		                              TIdent viewModelIdentifier, 
		                              TMessage message)
			where TMessage : ViewModelMessage;

		TResult SynchronRequest<TResult, TIdent, TMessage>(string viewModelCollectionIdentfier, 
		                                                   TIdent viewModelIdentifier, 
		                                                   TMessage requestMessage) 
			where TMessage : ViewModelRequest;


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               viewModelMessageBus                                 ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////


		void RegisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage: ViewModelMessage;

		void DeregisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage : ViewModelMessage;

		void Send<TMessage>(TMessage message) 
			where TMessage : ViewModelMessage;
	}

}
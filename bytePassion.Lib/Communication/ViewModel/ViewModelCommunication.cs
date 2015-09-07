using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.State;


namespace bytePassion.Lib.Communication.ViewModel
{

	public class ViewModelCommunication<TMessageBase>
	{
		private readonly IMessageBus<TMessageBase> viewModelMessageBus;
		private readonly IViewModelCollections     viewModelCollections;
		private readonly IStateEngine              viewModelVariableEngine;
		

		public ViewModelCommunication(IMessageBus<TMessageBase> viewModelMessageBus, 
									  IStateEngine viewModelVariableEngine, 
									  IViewModelCollections viewModelCollections)
		{
			this.viewModelMessageBus = viewModelMessageBus;
			this.viewModelVariableEngine = viewModelVariableEngine;
			this.viewModelCollections = viewModelCollections;			
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               global variables                                    ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		public void RegisterGlobalViewModelVariable<TVariableType>(string identifier, TVariableType initialValue = default(TVariableType))
		{
			viewModelVariableEngine.RegisterState(identifier, initialValue);
		}

		public IGlobalState<TVariableType> GetGlobalViewModelVariable<TVariableType>(string identifier)
		{
			return viewModelVariableEngine.GetState<TVariableType>(identifier);
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
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TViewModel, TIdent>(collectionIdentifier);
			viewModelCollection.AddViewModel(viewModel);
		}

		public void DeregisterViewModelAtCollection<TViewModel, TIdent>(string collectionIdentifier, TViewModel viewModel)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TViewModel, TIdent>(collectionIdentifier);
			viewModelCollection.RemoveViewModel(viewModel);
		}

		public void SendTo<TViewModel, TIdent, TMessage>(string viewModelCollectionIdentifier, 
													     TIdent viewModelIdentifier, 
													     TMessage message)
			where TViewModel : IViewModelMessageHandler<TMessage>, IViewModelCollectionItem<TIdent>
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TViewModel, TIdent>(viewModelCollectionIdentifier);

			var viewModel = viewModelCollection.GetViewModel(viewModelIdentifier);

			if (viewModel != null)			
				viewModel.Process(message);			
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                                   ///////////
		/////////                               viewModelMessageBus                                 ///////////
		/////////                                                                                   ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////////////

		public void RegisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage: TMessageBase
		{
			viewModelMessageBus.RegisterMessageHandler(viewModelMessageHandler);
		}

		public void DeregisterViewModelMessageHandler<TMessage>(IViewModelMessageHandler<TMessage> viewModelMessageHandler)
			where TMessage : TMessageBase
		{
			viewModelMessageBus.DeregisterMessageHander(viewModelMessageHandler);
		}

		public void Send<TMessage>(TMessage message) 
			where TMessage : TMessageBase
		{
			viewModelMessageBus.Send(message);
		}
		
	}
}

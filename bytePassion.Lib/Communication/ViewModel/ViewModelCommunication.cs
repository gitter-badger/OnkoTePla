using System;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.Lib.Communication.ViewModel
{

	public class ViewModelCommunication : IViewModelCommunication
	{
		private readonly IMessageBus<ViewModelMessage> viewModelMessageBus;
		private readonly IViewModelCollections     viewModelCollections;
		private readonly IStateEngine              viewModelVariableEngine;
		

		public ViewModelCommunication(IMessageBus<ViewModelMessage> viewModelMessageBus, 
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

		public void RegisterGlobalReadOnlyViewModelVariable<TVariableType>(string identifier, TVariableType value)
		{
			viewModelVariableEngine.RegisterStateReadOnly(identifier, value);
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

		public TResult SynchronRequest<TResult, TIdent, TMessage>(string viewModelCollectionIdentifier, 
																  TIdent viewModelIdentifier, 
																  TMessage requestMessage)
			where TMessage : ViewModelRequest
		{
			var viewModelCollection = viewModelCollections.GetViewModelCollection<TIdent>(viewModelCollectionIdentifier);

			var viewModel = viewModelCollection.GetViewModel(viewModelIdentifier);

			var viewModelAsRequestHandler = viewModel as IViewModelRequestHandler<TMessage, TResult>;

			if (viewModelAsRequestHandler == null)
				throw new InvalidOperationException("there is no handler for this request");

			return viewModelAsRequestHandler.Process(requestMessage);
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

using System;
using System.Collections.Generic;


namespace bytePassion.Lib.Communication.ViewModel
{

	public class ViewModelCollections : IViewModelCollections
	{
		private readonly IDictionary<string, object> viewModelCollections;

		public ViewModelCollections()
		{
			viewModelCollections = new Dictionary<string, object>();
		}		
				

		public void CreateViewModelCollection<TViewModel, TIdent>(string identifier)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			if (!viewModelCollections.ContainsKey(identifier))
				viewModelCollections.Add(identifier, new ViewModelCollection<TViewModel, TIdent>());
			else
				throw new ArgumentException($"collection with name {identifier} is already registered");
		}


		public void RemoveViewModelCollection (string identifier)
		{
			if (viewModelCollections.ContainsKey(identifier))
				viewModelCollections.Remove(identifier);
		}


		public ViewModelCollection<TViewModel, TIdent> GetViewModelCollection<TViewModel, TIdent>(string identifier)
			where TViewModel : IViewModelCollectionItem<TIdent>
		{
			if (viewModelCollections.ContainsKey(identifier))
				return (ViewModelCollection<TViewModel, TIdent>)viewModelCollections[identifier];

			throw new ArgumentException($"collection with name {identifier} is not available");
		}

	}

}
namespace bytePassion.Lib.Communication.ViewModel
{

	public interface IViewModelCollections
	{

		void CreateViewModelCollection<TViewModel, TIdent>(string identifier)
			where TViewModel : IViewModelCollectionItem<TIdent>;

		void RemoveViewModelCollection(string identifier);

		ViewModelCollection<TViewModel, TIdent> GetViewModelCollection<TViewModel, TIdent>(string identifier) 
			where TViewModel : IViewModelCollectionItem<TIdent>;
	}

}
using System;


namespace bytePassion.Lib.Communication.ViewModel
{

	public interface IViewModelCollections {

		void CreateViewModelCollection<TViewModel, TIdent>(string identifier, 
		                                                   Func<TViewModel,TIdent,bool> viewModelSelectorFunc);

		ViewModelCollection<TViewModel, TIdent> GetViewModelCollection<TViewModel, TIdent>(string identifier);
	}

}
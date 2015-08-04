using System;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.Lib.Communication.ViewModel
{

	public class ViewModelCollection <TViewModel, TIdent>
	{
		private readonly Func<TViewModel, TIdent, bool> viewModelSelectorFunc;

		private readonly IList<TViewModel> viewModels;

		public ViewModelCollection(Func<TViewModel, TIdent, bool> viewModelSelectorFunc)
		{
			this.viewModelSelectorFunc = viewModelSelectorFunc;

			viewModels = new List<TViewModel>();
		}

		public void AddViewModel(TViewModel viewModel)
		{
			viewModels.Add(viewModel);
		}

		public void RemoveViewModel(TViewModel viewModel)
		{
			viewModels.Remove(viewModel);
		}

		public TViewModel GetViewModel(TIdent identifier)
		{
			return viewModels.FirstOrDefault(viewModel => viewModelSelectorFunc(viewModel, identifier));
		}		
	}

}
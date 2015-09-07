using System.Collections.Generic;
using System.Linq;


namespace bytePassion.Lib.Communication.ViewModel
{

	public class ViewModelCollection <TViewModel, TIdent> where TViewModel : IViewModelCollectionItem<TIdent>		
	{		
		private readonly IList<TViewModel> viewModels;

		public ViewModelCollection()
		{			
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
			return viewModels.FirstOrDefault(viewModel => viewModel.Identifier.Equals(identifier));
		}		
	}

}
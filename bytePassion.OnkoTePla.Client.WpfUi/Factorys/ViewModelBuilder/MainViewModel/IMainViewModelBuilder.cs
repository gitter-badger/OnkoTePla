using System;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;

namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel
{
	internal interface IMainViewModelBuilder
	{
		IMainViewModel Build(Action<string> errorCallback);
		void DisposeViewModel(IMainViewModel viewModelToDispose);
	}
}
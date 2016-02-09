using System;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel
{
	internal interface ITherapyPlaceRowViewModelBuilder
	{
		void RequestBuild(Action<ITherapyPlaceRowViewModel> viewModelAvailable, 
						  TherapyPlace therapyPlace, Room room, TherapyPlaceRowIdentifier location,
						  Action<string> errorCallback);
	}
}
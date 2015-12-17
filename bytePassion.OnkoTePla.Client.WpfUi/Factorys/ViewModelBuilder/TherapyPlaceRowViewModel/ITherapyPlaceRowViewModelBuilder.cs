using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel
{
    internal interface ITherapyPlaceRowViewModelBuilder
	{
		ITherapyPlaceRowViewModel Build(TherapyPlace therapyPlace, Room room, TherapyPlaceRowIdentifier location);
	}
}
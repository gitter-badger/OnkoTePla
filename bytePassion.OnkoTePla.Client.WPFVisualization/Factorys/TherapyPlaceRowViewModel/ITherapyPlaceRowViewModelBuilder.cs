using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.TherapyPlaceRowViewModel
{
	public interface ITherapyPlaceRowViewModelBuilder
	{
		ITherapyPlaceRowViewModel Build(TherapyPlace therapyPlace, Room room, TherapyPlaceRowIdentifier location);
	}
}
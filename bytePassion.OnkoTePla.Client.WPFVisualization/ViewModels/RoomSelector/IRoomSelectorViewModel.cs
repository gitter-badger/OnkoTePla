using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector
{
	public interface IRoomSelectorViewModel : IViewModelBase
	{
		ObservableCollection<RoomSelectorData> AvailableRoomData { get; }

		RoomSelectorData SelectedOption { get; set; }
	}
}

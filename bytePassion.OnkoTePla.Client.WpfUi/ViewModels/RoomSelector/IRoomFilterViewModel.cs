using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector.Helper;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector
{
    public interface IRoomFilterViewModel : IViewModel
	{
		ObservableCollection<RoomSelectorData> AvailableRoomFilters { get; }

		RoomSelectorData SelectedRoomFilter { get; set; }


	}
}

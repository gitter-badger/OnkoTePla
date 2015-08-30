using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector
{
	class RoomSelectorViewModelSampleData : IRoomSelectorViewModel
	{
		public RoomSelectorViewModelSampleData()
		{
			AvailableRoomData = new ObservableCollection<RoomSelectorData>
			{
				new RoomSelectorData("room 01",    null, Colors.Aqua),
				new RoomSelectorData("room 02",    null, Colors.Coral),
				new RoomSelectorData("alle Räume", null, Colors.DarkGreen)
			};

			SelectedOption = AvailableRoomData[0];
		}

		public ObservableCollection<RoomSelectorData> AvailableRoomData { get; }

		public RoomSelectorData SelectedOption { get; set;  }		

		public event PropertyChangedEventHandler PropertyChanged;	
	}
}

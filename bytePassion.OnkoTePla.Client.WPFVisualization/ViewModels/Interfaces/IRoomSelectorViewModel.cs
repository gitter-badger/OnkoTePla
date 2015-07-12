﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface IRoomSelectorViewModel : IViewModelBase
	{
		ObservableCollection<RoomSelectorData> AvailableRoomData { get; }

		RoomSelectorData SelectedOption { get; set; }

		ICommand SelectAllRooms { get; }
	}
}
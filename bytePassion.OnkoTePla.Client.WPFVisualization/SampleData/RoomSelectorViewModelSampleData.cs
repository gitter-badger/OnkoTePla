﻿
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.SampleData
{
	class RoomSelectorViewModelSampleData : IRoomSelectorViewModel
	{
		public RoomSelectorViewModelSampleData()
		{
			AvailableRoomData = new ObservableCollection<RoomSelectorData>
			{
				new RoomSelectorData(new Room(new Guid(), "room 01", null, Colors.PaleVioletRed), null),
				new RoomSelectorData(new Room(new Guid(), "room 02", null, Colors.Orchid),        null),
				new RoomSelectorData(new Room(new Guid(), "room 03", null, Colors.LightSkyBlue),  null)
			};

			SelectedOption = AvailableRoomData[0];
		}

		public ObservableCollection<RoomSelectorData> AvailableRoomData
		{
			get; private set; 
		}

		public RoomSelectorData SelectedOption
		{
			get; set; 
		}

		public ICommand SelectAllRooms { get { return null; }}

		public event PropertyChangedEventHandler PropertyChanged;	
	}
}

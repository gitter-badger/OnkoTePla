using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.State;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class RoomSelectorViewModel : IRoomSelectorViewModel
	{

		private readonly GlobalState<Guid?> selectedRoomState;
		private readonly GlobalState<Tuple<Guid, uint>> displayedPracticeState;
		private readonly IConfigurationReadRepository configuration;		

		private readonly Command selectAllRoomsCommand;

		private ObservableCollection<RoomSelectorData> availableRoomData;
		private RoomSelectorData selectedOption;

		public RoomSelectorViewModel(GlobalState<Guid?> selectedRoomState,
									 GlobalState<Tuple<Guid, uint>> displayedPracticeState,									
									 IConfigurationReadRepository configuration)
		{
			this.selectedRoomState = selectedRoomState;
			this.displayedPracticeState = displayedPracticeState;
			this.configuration = configuration;

			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;
			
			selectAllRoomsCommand = new Command(ExecuteSelectAllRoom);

			OnDisplayedPracticeStateChanged(displayedPracticeState.Value);
		}

		private void OnDisplayedPracticeStateChanged (Tuple<Guid, uint> practiceInfo)
		{
			var roomData = configuration.GetMedicalPracticeByIdAndVersion(practiceInfo.Item1, practiceInfo.Item2)
										.Rooms
										.Select(room => new RoomSelectorData(room, new Command(() => SelectOption(room))));

			AvailableRoomData = new ObservableCollection<RoomSelectorData>(roomData);
		}

		private void SelectOption(Room room)
		{
			selectedRoomState.Value = room.Id;
			SelectedOption = availableRoomData.First(roomData => roomData.Room.Equals(room)); // TODO nötig?!?
		}

		private void ExecuteSelectAllRoom()
		{			
			selectedRoomState.Value = null;
			SelectedOption = RoomSelectorData.Dummy;
		}

		public ObservableCollection<RoomSelectorData> AvailableRoomData
		{
			get { return availableRoomData; }
			private set { PropertyChanged.ChangeAndNotify(this, ref availableRoomData, value);}
		}

		public RoomSelectorData SelectedOption
		{
			get { return selectedOption; }
			set {PropertyChanged.ChangeAndNotify(this, ref selectedOption, value); }
		}

		public ICommand SelectAllRooms
		{
			get { return selectAllRoomsCommand; }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

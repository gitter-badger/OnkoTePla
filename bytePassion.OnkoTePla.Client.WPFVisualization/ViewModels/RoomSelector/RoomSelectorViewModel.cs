using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess.Global;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector
{
	public class RoomSelectorViewModel : IRoomSelectorViewModel
	{

		private readonly IConfigurationReadRepository configuration;


		private IList<Room> currentSelectableRooms;
		private ObservableCollection<RoomSelectorData> availableRoomData;


		private readonly GlobalState<Guid?> selectedRoomState;
				
		
		
		private RoomSelectorData selectedOption;

		public RoomSelectorViewModel(IConfigurationReadRepository configuration)
		{
			this.configuration = configuration;

			selectedRoomState = ViewModelCommunication.GetGlobalViewModelVariable<Guid?>(AppointmentGridSelectedRoomVariable);	
					
			var displayedPracticeState = ViewModelCommunication.GetGlobalViewModelVariable<Tuple<Guid, uint>>(AppointmentGridDisplayedPracticeVariable);
			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;						

			OnDisplayedPracticeStateChanged(displayedPracticeState.Value);
		}

		private void OnDisplayedPracticeStateChanged (Tuple<Guid, uint> practiceInfo)
		{
			currentSelectableRooms = configuration.GetMedicalPracticeByIdAndVersion(practiceInfo.Item1, practiceInfo.Item2)
				                                  .Rooms
												  .ToList();

			AvailableRoomData = currentSelectableRooms.Select(room => new RoomSelectorData(room.Name, room.Id, room.DisplayedColor))
													  .Append(new RoomSelectorData("Alle Räume", null, Colors.White))
											          .ToObservableCollection();
			SelectedOption = AvailableRoomData.Last();
		}
		
	
		public ObservableCollection<RoomSelectorData> AvailableRoomData
		{
			get { return availableRoomData; }
			private set { PropertyChanged.ChangeAndNotify(this, ref availableRoomData, value);}
		}

		public RoomSelectorData SelectedOption
		{
			get { return selectedOption; }
			set
			{
				if (value == null) return; // TODO: weiß nicht genau warum das hier nötig ist ......

				if (value != selectedOption)
				{
					selectedRoomState.Value = value.RoomId;
				}

				PropertyChanged.ChangeAndNotify(this, ref selectedOption, value);
			}
		}		

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

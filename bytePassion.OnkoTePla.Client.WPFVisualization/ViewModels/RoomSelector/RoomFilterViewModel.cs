using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector
{
	public class RoomFilterViewModel : IRoomFilterViewModel
	{
		private readonly RoomSelectorData allRoomFilter = new RoomSelectorData("Alle Räume", null, Colors.White);

		private readonly IDataCenter dataCenter;		

		private readonly GlobalState<Guid?> roomFilter;
		private readonly GlobalState<Date>  selectedDate;
		private readonly GlobalState<Guid>  displayedMedicalPractice;

		private IList<Room> currentSelectableRoomFilters;
		private RoomSelectorData selectedRoomFilter;
		private ObservableCollection<RoomSelectorData> availableRoomFilters;


		public RoomFilterViewModel(IDataCenter dataCenter, 
								   ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{
			this.dataCenter = dataCenter;

			roomFilter = viewModelCommunication.GetGlobalViewModelVariable<Guid?>(
				AppointmentGridRoomFilterVariable
			);

			selectedDate = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);

			displayedMedicalPractice = viewModelCommunication.GetGlobalViewModelVariable<Guid>(
				AppointmentGridDisplayedPracticeVariable	
			);


			roomFilter.StateChanged += OnRoomFilterChanged;
			displayedMedicalPractice.StateChanged += OnDisplayedPracticeStateChanged;						
			selectedDate.StateChanged += OnSelectedDateChanged;

			SetRoomData(selectedDate.Value, displayedMedicalPractice.Value);
		}

		private void OnRoomFilterChanged(Guid? guid)
		{
			if (guid == null)
				SelectedRoomFilter = allRoomFilter;
			else
			{
				var room = dataCenter.GetMedicalPracticeByDateAndId(selectedDate.Value, displayedMedicalPractice.Value)
					                 .GetRoomById(guid.Value);

				selectedRoomFilter = new RoomSelectorData(room.Name, room.Id, room.DisplayedColor);
				PropertyChanged.Notify(this, nameof(SelectedRoomFilter));
			}
		}

		private void OnSelectedDateChanged(Date date)
		{			
			SetRoomData(date, displayedMedicalPractice.Value);
		}
		
		private void OnDisplayedPracticeStateChanged (Guid medicalPracticeId)
		{			
			SetRoomData(selectedDate.Value, medicalPracticeId);
		}

		private void SetRoomData (Date date, Guid medicalPracticeId)
		{
			var correctMedicalPractice = dataCenter.GetMedicalPracticeByDateAndId(date, medicalPracticeId);

			currentSelectableRoomFilters = correctMedicalPractice.Rooms.ToList();

			AvailableRoomFilters = currentSelectableRoomFilters.Select(room => new RoomSelectorData(room.Name, room.Id, room.DisplayedColor))
															   .Append(allRoomFilter)
															   .ToObservableCollection();

			SelectedRoomFilter = AvailableRoomFilters.Last();
		}


		public ObservableCollection<RoomSelectorData> AvailableRoomFilters
		{
			get { return availableRoomFilters; }
			private set { PropertyChanged.ChangeAndNotify(this, ref availableRoomFilters, value);}
		}

		public RoomSelectorData SelectedRoomFilter
		{
			get { return selectedRoomFilter; }
			set
			{
				if (value == null) return; // TODO: weiß nicht genau warum das hier nötig ist ......

				if (value != selectedRoomFilter)
				{
					roomFilter.Value = value.RoomId;
				}

				PropertyChanged.ChangeAndNotify(this, ref selectedRoomFilter, value);
			}
		}		

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

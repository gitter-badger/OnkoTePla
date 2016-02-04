using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector
{
	internal class RoomFilterViewModel : ViewModel,
                                         IRoomFilterViewModel
	{
		private readonly RoomSelectorData allRoomFilter = new RoomSelectorData("Alle Räume", null, Colors.White);

		private readonly IDataCenter dataCenter;		

		private readonly ISharedState<Guid?>         roomFilterVariable;
		private readonly ISharedStateReadOnly<Date>  selectedDateVariable;
		private readonly ISharedStateReadOnly<Guid>  displayedMedicalPracticeVariable;
        
		private IList<Room> currentSelectableRoomFilters;
		private RoomSelectorData selectedRoomFilter;
		private ObservableCollection<RoomSelectorData> availableRoomFilters;


		public RoomFilterViewModel(IDataCenter dataCenter,
								   ISharedState<Guid?> roomFilterVariable, 
                                   ISharedStateReadOnly<Date> selectedDateVariable, 
                                   ISharedStateReadOnly<Guid> displayedMedicalPracticeVariable)
		{
			this.dataCenter = dataCenter;
		    this.roomFilterVariable = roomFilterVariable;
		    this.selectedDateVariable = selectedDateVariable;
		    this.displayedMedicalPracticeVariable = displayedMedicalPracticeVariable;

		    roomFilterVariable.StateChanged += OnRoomFilterVariableChanged;
			displayedMedicalPracticeVariable.StateChanged += OnDisplayedPracticeVariableStateChanged;						
			selectedDateVariable.StateChanged += OnSelectedDateVariableChanged;

			SetRoomData(selectedDateVariable.Value, displayedMedicalPracticeVariable.Value);
		}

		private void OnRoomFilterVariableChanged(Guid? guid)
		{
			if (guid == null)
				SelectedRoomFilter = allRoomFilter;
			else
			{
				var room = dataCenter.GetMedicalPracticeByIdAndDate(displayedMedicalPracticeVariable.Value, selectedDateVariable.Value)
					                 .GetRoomById(guid.Value);

				selectedRoomFilter = new RoomSelectorData(room.Name, room.Id, room.DisplayedColor);
				PropertyChanged.Notify(this, nameof(SelectedRoomFilter));
			}
		}

		private void OnSelectedDateVariableChanged(Date date)
		{			
			SetRoomData(date, displayedMedicalPracticeVariable.Value);
		}
		
		private void OnDisplayedPracticeVariableStateChanged (Guid medicalPracticeId)
		{			
			SetRoomData(selectedDateVariable.Value, medicalPracticeId);
		}

		private void SetRoomData (Date date, Guid medicalPracticeId)
		{
			var correctMedicalPractice = dataCenter.GetMedicalPracticeByIdAndDate(medicalPracticeId, date);

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
					roomFilterVariable.Value = value.RoomId;
				}

				PropertyChanged.ChangeAndNotify(this, ref selectedRoomFilter, value);
			}
		}


        protected override void CleanUp()
        {
            roomFilterVariable.StateChanged               -= OnRoomFilterVariableChanged;
            displayedMedicalPracticeVariable.StateChanged -= OnDisplayedPracticeVariableStateChanged;
            selectedDateVariable.StateChanged             -= OnSelectedDateVariableChanged;
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}

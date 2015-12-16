using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Model;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector
{
    public class RoomFilterViewModel : ViewModel,
                                       IRoomFilterViewModel
	{
		private readonly RoomSelectorData allRoomFilter = new RoomSelectorData("Alle Räume", null, Colors.White);

		private readonly IDataCenter dataCenter;		

		private readonly IGlobalState<Guid?>         roomFilterVariable;
		private readonly IGlobalStateReadOnly<Date>  selectedDateVariable;
		private readonly IGlobalStateReadOnly<Guid>  displayedMedicalPracticeVariable;
        
		private IList<Room> currentSelectableRoomFilters;
		private RoomSelectorData selectedRoomFilter;
		private ObservableCollection<RoomSelectorData> availableRoomFilters;


		public RoomFilterViewModel(IDataCenter dataCenter,
								   IGlobalState<Guid?> roomFilterVariable, 
                                   IGlobalStateReadOnly<Date> selectedDateVariable, 
                                   IGlobalStateReadOnly<Guid> displayedMedicalPracticeVariable)
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
				var room = dataCenter.GetMedicalPracticeByDateAndId(selectedDateVariable.Value, displayedMedicalPracticeVariable.Value)
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

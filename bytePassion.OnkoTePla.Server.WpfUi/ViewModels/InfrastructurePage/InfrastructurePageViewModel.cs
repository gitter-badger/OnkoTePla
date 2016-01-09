using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
	internal class InfrastructurePageViewModel : ViewModel, 
                                                 IInfrastructurePageViewModel
    {                     
	    public ObservableCollection<MedicalPractice> MedicalPractices { get; }
	    public ObservableCollection<Room> Rooms { get; }
	    public ObservableCollection<TherapyPlace> TherapyPlaces { get; }

	    public ICommand AddMedicalPractice { get; }
		public ICommand SaveMedicalPracticeChanges { get; }
		public ICommand DeleteMedicalPractice { get; }
		public ICommand AddRoom { get; }
		public ICommand SaveRoomChanges { get; }
		public ICommand DeleteRoom { get; }
		public ICommand AddTherapyPlace { get; }
		public ICommand SaveTherapyPlaceChanges { get; }
		public ICommand DeleteTherapyPlace { get; }

		public MedicalPractice SelectedMedicalPractice { get; set; }
	    public Room SelectedRoom { get; set; }
	    public TherapyPlace SelectedTherapyPlace { get; set; }
		public string PracticeName { get; set; }
		public string RoomName { get; set; }
		public string TherapyPlaceName { get; set; }
		public ColorDisplayData RoomDisplayColor { get; set; }
		public TherapyPlaceTypeDisplayData TherapyPlaceType { get; set; }
		public bool IsRoomListVisible { get; }
		public bool IsTherapyPlaceListVisible { get; }
		public bool IsMedPracticeSettingVisible { get; }
		public bool IsRoomSettingVisible { get; }
		public bool IsTherapyPlaceSettingVisible { get; }
		public ObservableCollection<ColorDisplayData> AvailableColors { get; }
		public ObservableCollection<TherapyPlaceTypeDisplayData> AvailableTherapyPlaceTypes { get; }

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
	internal class InfrastructurePageViewModelSampleData : IInfrastructurePageViewModel
    {
	    public InfrastructurePageViewModelSampleData()
	    {		    
			MedicalPractices = new ObservableCollection<MedicalPractice>
			{
				MedicalPracticeCreateAndEditLogic.Create("practice1"),
				MedicalPracticeCreateAndEditLogic.Create("practice2"),
				MedicalPracticeCreateAndEditLogic.Create("practice3")
			};

			Rooms = new ObservableCollection<Room>
			{
				RoomCreateAndEditLogic.Create("room1"),
				RoomCreateAndEditLogic.Create("room2")
			};

		    TherapyPlaces = new ObservableCollection<TherapyPlace>
		    {
				TherapyPlaceCreateAndEditLogic.Create("place1"),
				TherapyPlaceCreateAndEditLogic.Create("place2"),
				TherapyPlaceCreateAndEditLogic.Create("place3"),
				TherapyPlaceCreateAndEditLogic.Create("place4"),
				TherapyPlaceCreateAndEditLogic.Create("place5"),
				TherapyPlaceCreateAndEditLogic.Create("place6")							
			};

			AvailableColors = new ObservableCollection<ColorDisplayData>
			{
				new ColorDisplayData(Colors.Aqua),
				new ColorDisplayData(Colors.Blue),
				new ColorDisplayData(Colors.Fuchsia)
			};

			const string iconBasePath = "pack://application:,,,/bytePassion.OnkoTePla.Resources;component/Icons/TherapyPlaceType/";

			AvailableTherapyPlaceTypes = new ObservableCollection<TherapyPlaceTypeDisplayData>
			{
				new TherapyPlaceTypeDisplayData("none" , ImageLoader.LoadImage(new Uri(iconBasePath + "none.png"))),
				new TherapyPlaceTypeDisplayData("type1", ImageLoader.LoadImage(new Uri(iconBasePath + "bed02.png"))),
				new TherapyPlaceTypeDisplayData("type2", ImageLoader.LoadImage(new Uri(iconBasePath + "bed04.png")))
			};

		    SelectedMedicalPractice = MedicalPractices.First();
		    SelectedRoom            = Rooms.First();
		    SelectedTherapyPlace    = TherapyPlaces.First();

		    PracticeName     = SelectedMedicalPractice.Name;
		    RoomName         = SelectedRoom.Name;
		    TherapyPlaceName = SelectedTherapyPlace.Name;

		    RoomDisplayColor = AvailableColors.First();
		    TherapyPlaceType = AvailableTherapyPlaceTypes.First();

		    IsRoomListVisible            = true;
			IsTherapyPlaceListVisible    = true;
			IsMedPracticeSettingVisible  = true;
			IsRoomSettingVisible         = true;
			IsTherapyPlaceSettingVisible = true;

		}

	    public ObservableCollection<MedicalPractice> MedicalPractices { get; }
	    public ObservableCollection<Room>            Rooms            { get; }
	    public ObservableCollection<TherapyPlace>    TherapyPlaces    { get; }

	    public ICommand AddMedicalPractice         => null;
		public ICommand SaveMedicalPracticeChanges => null;
		public ICommand DeleteMedicalPractice      => null;
		public ICommand AddRoom                    => null;
		public ICommand SaveRoomChanges            => null;
		public ICommand DeleteRoom                 => null;
		public ICommand AddTherapyPlace            => null;
		public ICommand SaveTherapyPlaceChanges    => null;
		public ICommand DeleteTherapyPlace         => null;

		public MedicalPractice SelectedMedicalPractice { get; set; }
	    public Room            SelectedRoom            { get; set; }
	    public TherapyPlace    SelectedTherapyPlace    { get; set; }

		public string PracticeName     { get; set; }
		public string RoomName         { get; set; }
		public string TherapyPlaceName { get; set; }
		
		public ColorDisplayData            RoomDisplayColor { get; set; }
		public TherapyPlaceTypeDisplayData TherapyPlaceType { get; set; }

		public bool IsRoomListVisible            { get; }
		public bool IsTherapyPlaceListVisible    { get; }
		public bool IsMedPracticeSettingVisible  { get; }
		public bool IsRoomSettingVisible         { get; }
		public bool IsTherapyPlaceSettingVisible { get; }

		public ObservableCollection<ColorDisplayData>            AvailableColors            { get; }
		public ObservableCollection<TherapyPlaceTypeDisplayData> AvailableTherapyPlaceTypes { get; } 

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
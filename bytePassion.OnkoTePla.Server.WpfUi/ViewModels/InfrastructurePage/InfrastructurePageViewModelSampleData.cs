using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
	internal class InfrastructurePageViewModelSampleData : IInfrastructurePageViewModel
    {
	    public InfrastructurePageViewModelSampleData()
	    {
		    IsAnyTherapyPlaceTypeConfigured = true;

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
				TherapyPlaceCreateAndEditLogic.Create("place6"),
				TherapyPlaceCreateAndEditLogic.Create("place7"),
				TherapyPlaceCreateAndEditLogic.Create("place8"),
				TherapyPlaceCreateAndEditLogic.Create("place9")				
			};

		    SelectedMedicalPractice = MedicalPractices.First();
		    SelectedRoom            = Rooms.First();
		    SelectedTherapyPlace    = TherapyPlaces.First();

		    IsAnyTherapyPlaceTypeConfigured = true;
	    }

	    public ObservableCollection<MedicalPractice> MedicalPractices { get; }
	    public ObservableCollection<Room>            Rooms            { get; }
	    public ObservableCollection<TherapyPlace>    TherapyPlaces    { get; }

	    public ICommand AddMedicalPractice => null;
	    public ICommand AddRoom            => null;
	    public ICommand AddTherapyPlace    => null;

	    public MedicalPractice SelectedMedicalPractice { get; set; }
	    public Room            SelectedRoom            { get; set; }
	    public TherapyPlace    SelectedTherapyPlace    { get; set; }

	    public bool IsAnyTherapyPlaceTypeConfigured { get; }

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
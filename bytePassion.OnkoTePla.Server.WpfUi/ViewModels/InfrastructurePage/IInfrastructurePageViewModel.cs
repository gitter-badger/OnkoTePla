using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
	internal interface IInfrastructurePageViewModel : IViewModel
    {
	    ObservableCollection<MedicalPractice> MedicalPractices { get; }
		ObservableCollection<Room>            Rooms            { get; } 
		ObservableCollection<TherapyPlace>    TherapyPlaces    { get; } 

		ICommand AddMedicalPractice { get; }
		ICommand AddRoom            { get; }
		ICommand AddTherapyPlace    { get; }

		MedicalPractice SelectedMedicalPractice { get; set; }
		Room            SelectedRoom            { get; set; }
		TherapyPlace    SelectedTherapyPlace    { get; set; }

		bool IsAnyTherapyPlaceTypeConfigured { get; }
    }
}
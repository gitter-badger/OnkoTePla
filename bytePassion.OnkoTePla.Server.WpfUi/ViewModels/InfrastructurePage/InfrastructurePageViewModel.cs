using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage
{
	internal class InfrastructurePageViewModel : ViewModel, 
                                                 IInfrastructurePageViewModel
    {                     
	    public ObservableCollection<MedicalPractice> MedicalPractices { get; }
	    public ObservableCollection<Room> Rooms { get; }
	    public ObservableCollection<TherapyPlace> TherapyPlaces { get; }
	    public ICommand AddMedicalPractice { get; }
	    public ICommand AddRoom { get; }
	    public ICommand AddTherapyPlace { get; }
	    public MedicalPractice SelectedMedicalPractice { get; set; }
	    public Room SelectedRoom { get; set; }
	    public TherapyPlace SelectedTherapyPlace { get; set; }
	    public bool IsAnyTherapyPlaceTypeConfigured { get; }

		protected override void CleanUp () { }
		public override event PropertyChangedEventHandler PropertyChanged;
	}
}

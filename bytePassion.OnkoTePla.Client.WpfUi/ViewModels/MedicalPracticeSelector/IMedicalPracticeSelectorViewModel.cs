using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector
{
    public interface IMedicalPracticeSelectorViewModel : IViewModel
	{
		MedicalPractice SelectedMedicalPractice { get; set; }

		ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; } 

		bool PracticeIsSelectable { get; }
	}
}

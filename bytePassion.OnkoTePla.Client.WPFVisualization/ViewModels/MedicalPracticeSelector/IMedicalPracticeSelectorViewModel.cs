using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
{
	public interface IMedicalPracticeSelectorViewModel : IViewModel
	{
		MedicalPractice SelectedMedicalPractice { get; set; }

		ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; } 

		bool PracticeIsSelectable { get; }
	}
}

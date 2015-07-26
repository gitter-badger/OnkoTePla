using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
{
	public interface IMedicalPracticeSelectorViewModel : IViewModelBase
	{
		MedicalPractice SelectedMedicalPractice { get; set; }

		ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; } 
	}
}

using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelectorViewModel
{
	public interface IMedicalPracticeSelectorViewModel : IViewModelBase
	{
		MedicalPractice SelectedMedicalPractice { get; set; }

		ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; } 
	}
}

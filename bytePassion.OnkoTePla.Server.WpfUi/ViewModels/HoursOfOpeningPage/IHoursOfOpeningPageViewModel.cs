using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage
{
	internal interface IHoursOfOpeningPageViewModel : IViewModel
	{		
		ObservableCollection<MedPracticeDisplayData> AvailableMedicalPractices { get; } 

		MedPracticeDisplayData SelectedMedicalPractice { get; set; }

		bool IsHoursOfOpeningSettingVisible { get; }
		bool IsAnyPracticeAvailable { get; }
	}
}
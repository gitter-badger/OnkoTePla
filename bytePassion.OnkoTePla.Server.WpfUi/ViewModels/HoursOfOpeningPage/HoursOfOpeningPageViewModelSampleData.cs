using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage
{
	internal class HoursOfOpeningPageViewModelSampleData : IHoursOfOpeningPageViewModel
	{
		public HoursOfOpeningPageViewModelSampleData()
		{
			AvailableMedicalPractices = new ObservableCollection<MedPracticeDisplayData>
			{
				new MedPracticeDisplayData("practice1", Guid.NewGuid()),
				new MedPracticeDisplayData("practice2", Guid.NewGuid()),
				new MedPracticeDisplayData("practice3", Guid.NewGuid())
			};
			SelectedMedicalPractice = AvailableMedicalPractices.First();
			IsAnyPracticeAvailable = true;
			IsHoursOfOpeningSettingVisible = true;
		}

		public ObservableCollection<MedPracticeDisplayData> AvailableMedicalPractices { get; }

		public MedPracticeDisplayData SelectedMedicalPractice { get; set; }

		public bool IsHoursOfOpeningSettingVisible { get; }
		public bool IsAnyPracticeAvailable { get; }

		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
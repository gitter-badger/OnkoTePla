using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage
{
	internal class HoursOfOpeningPageViewModel : ViewModel, 
												 IHoursOfOpeningPageViewModel
	{
		private readonly IDataCenter dataCenter;
		private readonly IGlobalStateReadOnly<MainPage> selectedPageVariable;

		private bool isHoursOfOpeningSettingVisible;
		private bool isAnyPracticeAvailable;

		public HoursOfOpeningPageViewModel(IDataCenter dataCenter,
										   IGlobalStateReadOnly<MainPage> selectedPageVariable)
		{
			this.dataCenter = dataCenter;
			this.selectedPageVariable = selectedPageVariable;

			AvailableMedicalPractices = new ObservableCollection<MedPracticeDisplayData>();

			IsHoursOfOpeningSettingVisible = false;
			IsAnyPracticeAvailable = false;

			selectedPageVariable.StateChanged += OnSelectedPageStateChanged;
		}

		private void OnSelectedPageStateChanged(MainPage mainPage)
		{
			if (mainPage == MainPage.HoursOfOpening)
			{
				AvailableMedicalPractices.Clear();

				dataCenter.GetAllMedicalPractices()
						  .Select(medPractice => new MedPracticeDisplayData(medPractice.Name, medPractice.Id))
						  .Do(AvailableMedicalPractices.Add);
			}
		}

		public ObservableCollection<MedPracticeDisplayData> AvailableMedicalPractices { get; }

		public MedPracticeDisplayData SelectedMedicalPractice { get; set; }

		public bool IsHoursOfOpeningSettingVisible
		{
			get { return isHoursOfOpeningSettingVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isHoursOfOpeningSettingVisible, value); }
		}

		public bool IsAnyPracticeAvailable
		{
			get { return isAnyPracticeAvailable; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isAnyPracticeAvailable, value); }
		}

		protected override void CleanUp()
		{
			selectedPageVariable.StateChanged -= OnSelectedPageStateChanged;
		}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}

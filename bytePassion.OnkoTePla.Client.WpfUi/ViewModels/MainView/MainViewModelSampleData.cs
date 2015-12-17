using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView
{
    public class MainViewModelSampleData : IMainViewModel
	{
		public MainViewModelSampleData()
		{
			SelectedPage = 0;

			OverviewPageViewModel = new OverviewPageViewModelSampleData();
			SearchPageViewModel   = new SearchPageViewModelSampleData();
			OptionsPageViewModel  = new OptionsPageViewModelSampleData();
			
			NotificationServiceContainerViewModel = new NotificationServiceContainerViewModelSampleData();		
		}

		public int SelectedPage { get; }

		public ICommand ShowPage { get; } = null;

		public IOverviewPageViewModel OverviewPageViewModel { get; }
		public ISearchPageViewModel   SearchPageViewModel   { get; }
		public IOptionsPageViewModel  OptionsPageViewModel  { get; }

		public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }

		public void Process (ShowPage message) { }
		
	    public void Dispose() {	}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

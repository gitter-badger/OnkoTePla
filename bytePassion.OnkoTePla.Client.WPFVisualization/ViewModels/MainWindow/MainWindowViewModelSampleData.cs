using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow
{
	public class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
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

using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow
{
	interface INewMainWindowViewModel : IViewModel
										
	{
		int SelectedPage { get; }

		ICommand ShowOverviewPage { get; }
		ICommand ShowSearchPage   { get; }
		ICommand ShowOptionsPage  { get; }

		IOverviewPageViewModel OverviewPageViewModel { get; }
		ISearchPageViewModel   SearchPageViewModel   { get; }
		IOptionsPageViewModel  OptionsPageViewModel  { get; }	
					
		INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }
	}
}

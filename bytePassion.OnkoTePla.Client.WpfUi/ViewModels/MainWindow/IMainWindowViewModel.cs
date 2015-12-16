using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow
{
	interface IMainWindowViewModel : IViewModel,
									 IViewModelMessageHandler<ShowPage>

	{
		int SelectedPage { get; }

		ICommand ShowPage { get; }		

		IOverviewPageViewModel OverviewPageViewModel { get; }
		ISearchPageViewModel   SearchPageViewModel   { get; }
		IOptionsPageViewModel  OptionsPageViewModel  { get; }	
					
		INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }
	}
}

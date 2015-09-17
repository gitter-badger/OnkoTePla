using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow
{
	interface INewMainWindowViewModel : IViewModel,
										IViewModelMessageHandler<ShowNotification>,
										IViewModelMessageHandler<HideNotification>
	{
		int SelectedPage { get; }

		ICommand ShowOverviewPage { get; }
		ICommand ShowSearchPage   { get; }
		ICommand ShowOptionsPage  { get; }

		IOverviewPageViewModel OverviewPageViewModel { get; }
		ISearchPageViewModel   SearchPageViewModel   { get; }
		IOptionsPageViewModel  OptionsPageViewModel  { get; }
		
		bool NotificationVisible { get; }

		INotificationViewModel NotificationViewModel { get; }
	}
}

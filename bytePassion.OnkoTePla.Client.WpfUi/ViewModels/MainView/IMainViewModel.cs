using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView
{
    internal interface IMainViewModel : IViewModel,
							            IViewModelMessageHandler<ShowPage>

	{
		int SelectedPage { get; }

		ICommand ShowPage { get; }		

		IOverviewPageViewModel OverviewPageViewModel { get; }
		ISearchPageViewModel   SearchPageViewModel   { get; }
		IOptionsPageViewModel  OptionsPageViewModel  { get; }								
	}
}

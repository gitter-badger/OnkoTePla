using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow
{
	public class NewMainWindowViewModel : INewMainWindowViewModel
	{
		private int selectedPage;

		public NewMainWindowViewModel(IOverviewPageViewModel overviewPageViewModel, 
									  ISearchPageViewModel searchPageViewModel, 
									  IOptionsPageViewModel optionsPageViewModel)
		{
			OverviewPageViewModel = overviewPageViewModel;
			SearchPageViewModel   = searchPageViewModel;
			OptionsPageViewModel  = optionsPageViewModel;

			ShowOverviewPage = new Command(() => SelectedPage = 0);
			ShowSearchPage   = new Command(() => SelectedPage = 1);
			ShowOptionsPage  = new Command(() => SelectedPage = 2);
		}

		public int SelectedPage
		{
			get { return selectedPage; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
		}

		public ICommand ShowOverviewPage { get; }
		public ICommand ShowSearchPage   { get; }
		public ICommand ShowOptionsPage  { get; }

		public IOverviewPageViewModel OverviewPageViewModel { get; }
		public ISearchPageViewModel   SearchPageViewModel   { get; }
		public IOptionsPageViewModel  OptionsPageViewModel  { get; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

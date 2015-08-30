using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow
{
	public class NewMainWindowViewModelSampleData : INewMainWindowViewModel
	{
		public NewMainWindowViewModelSampleData()
		{
			SelectedPage = 0;

			OverviewPageViewModel = new OverviewPageViewModelSampleData();
			SearchPageViewModel   = new SearchPageViewModelSampleData();
			OptionsPageViewModel  = new OptionsPageViewModelSampleData();
		}

		public int SelectedPage { get; }

		public ICommand ShowOverviewPage { get; } = null;
		public ICommand ShowSearchPage   { get; } = null;
		public ICommand ShowOptionsPage  { get; } = null;

		public IOverviewPageViewModel OverviewPageViewModel { get; }
		public ISearchPageViewModel   SearchPageViewModel   { get; }
		public IOptionsPageViewModel  OptionsPageViewModel  { get; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

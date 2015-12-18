using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView
{
    internal class MainViewModel : ViewModel, 
                                   IMainViewModel
    {        
        private int selectedPage;        

        public MainViewModel(IOverviewPageViewModel overviewPageViewModel,
						     ISearchPageViewModel searchPageViewModel,
						     IOptionsPageViewModel optionsPageViewModel)
        {           
	        OverviewPageViewModel = overviewPageViewModel;
            SearchPageViewModel = searchPageViewModel;
            OptionsPageViewModel = optionsPageViewModel;

            SelectedPage = 0;
        }

        public int SelectedPage
        {
            get { return selectedPage; }
            private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
        }
				
        public IOverviewPageViewModel OverviewPageViewModel { get; }
        public ISearchPageViewModel   SearchPageViewModel   { get; }
        public IOptionsPageViewModel  OptionsPageViewModel  { get; }

		public void Process (ShowPage message)
		{
		    SelectedPage = (int) message.Page;
		}		
	            
	    protected override void CleanUp() {	}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
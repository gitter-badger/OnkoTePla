using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.Enums;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using System.ComponentModel;
using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow
{
    public class MainWindowViewModel : ViewModel, 
                                       IMainWindowViewModel
    {        
        private int selectedPage;        

        public MainWindowViewModel(IOverviewPageViewModel overviewPageViewModel,
								   ISearchPageViewModel searchPageViewModel,
								   IOptionsPageViewModel optionsPageViewModel,									 
								   INotificationServiceContainerViewModel notificationServiceContainerViewModel)
        {           
	        OverviewPageViewModel = overviewPageViewModel;
            SearchPageViewModel = searchPageViewModel;
            OptionsPageViewModel = optionsPageViewModel;

			NotificationServiceContainerViewModel = notificationServiceContainerViewModel;		  
			
			ShowPage = new ParameterrizedCommand<MainPage>(page => SelectedPage = (int)page);        
        }

        public int SelectedPage
        {
            get { return selectedPage; }
            private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
        }

		public ICommand ShowPage { get; }
		
        public IOverviewPageViewModel OverviewPageViewModel { get; }
        public ISearchPageViewModel   SearchPageViewModel   { get; }
        public IOptionsPageViewModel  OptionsPageViewModel  { get; }

		public void Process (ShowPage message)
		{
			ShowPage.Execute(message.Page);
		}

		public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }
	            
	    protected override void CleanUp() {	}
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
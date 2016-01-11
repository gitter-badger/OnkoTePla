using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
    {
        public MainWindowViewModelSampleData()
        {
            SelectedPage = MainPage.Overview;

            OverviewPageViewModel          = new OverviewPageViewModelSampleData();
            ConnectionsPageViewModel       = new ConnectionsPageViewModelSampleData();
            UserPageViewModel              = new UserPageViewModelSampleData();
            LicencePageViewModel           = new LicencePageViewModelSampleData();
            InfrastructurePageViewModel    = new InfrastructurePageViewModelSampleData();
			HoursOfOpeningPageViewModel    = new HoursOfOpeningPageViewModelSampleData();
            OptionsPageViewModel           = new OptionsPageViewModelSampleData();
            AboutPageViewModel             = new AboutPageViewModelSampleData();
			TherapyPlaceTypesPageViewModel = new TherapyPlaceTypesPageViewModelSampleData();
		}

        public ICommand SwitchToPage { get; } = null;
        public MainPage SelectedPage { get; }

        public IOverviewPageViewModel          OverviewPageViewModel          { get; }
        public IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        public IUserPageViewModel              UserPageViewModel              { get; }
        public ILicencePageViewModel           LicencePageViewModel           { get; }
        public IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
		public IHoursOfOpeningPageViewModel    HoursOfOpeningPageViewModel    { get; }
		public ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
	    public IOptionsPageViewModel           OptionsPageViewModel           { get; }
        public IAboutPageViewModel             AboutPageViewModel             { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
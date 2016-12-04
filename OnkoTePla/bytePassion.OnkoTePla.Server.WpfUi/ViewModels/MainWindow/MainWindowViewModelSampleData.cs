using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LabelPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage;
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
	        CheckWindowClosing = true;

            OverviewPageViewModel          = new OverviewPageViewModelSampleData();
            ConnectionsPageViewModel       = new ConnectionsPageViewModelSampleData();
            UserPageViewModel              = new UserPageViewModelSampleData();
            LicencePageViewModel           = new LicencePageViewModelSampleData();
            InfrastructurePageViewModel    = new InfrastructurePageViewModelSampleData();
			HoursOfOpeningPageViewModel    = new HoursOfOpeningPageViewModelSampleData();
            OptionsPageViewModel           = new OptionsPageViewModelSampleData();
            AboutPageViewModel             = new AboutPageViewModelSampleData();
			TherapyPlaceTypesPageViewModel = new TherapyPlaceTypesPageViewModelSampleData();
			PatientsPageViewModel		   = new PatientsPageViewModelSampleData();
			BackupPageViewModel			   = new BackupPageViewModelSampleData();
			LabelPageViewModel			   = new LabelPageViewModelSampleData();
		}

        public ICommand SwitchToPage => null;
        public MainPage SelectedPage { get; }

		public bool CheckWindowClosing { get; }
		public ICommand CloseWindow => null;
		
		public IOverviewPageViewModel          OverviewPageViewModel          { get; }
        public IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        public IUserPageViewModel              UserPageViewModel              { get; }
        public ILicencePageViewModel           LicencePageViewModel           { get; }
        public IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
		public IHoursOfOpeningPageViewModel    HoursOfOpeningPageViewModel    { get; }
		public ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
		public ILabelPageViewModel			   LabelPageViewModel			  { get; }
		public IPatientsPageViewModel          PatientsPageViewModel          { get; }
		public IBackupPageViewModel			   BackupPageViewModel			  { get; }
		public IOptionsPageViewModel           OptionsPageViewModel           { get; }
        public IAboutPageViewModel             AboutPageViewModel             { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
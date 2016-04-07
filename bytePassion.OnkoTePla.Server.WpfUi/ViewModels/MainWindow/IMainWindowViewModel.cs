using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.BackupPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
    {
        ICommand SwitchToPage { get; }
        MainPage SelectedPage { get; }

		bool CheckWindowClosing { get; }
		ICommand CloseWindow { get; }

		IOverviewPageViewModel          OverviewPageViewModel          { get; }
        IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        IUserPageViewModel              UserPageViewModel              { get; }
        ILicencePageViewModel           LicencePageViewModel           { get; }
        IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
		IHoursOfOpeningPageViewModel    HoursOfOpeningPageViewModel    { get; }
		ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
		IPatientsPageViewModel			PatientsPageViewModel		   { get; }
		IBackupPageViewModel			BackupPageViewModel			   { get; }
		IOptionsPageViewModel           OptionsPageViewModel           { get; }
        IAboutPageViewModel             AboutPageViewModel             { get; }
    }
}
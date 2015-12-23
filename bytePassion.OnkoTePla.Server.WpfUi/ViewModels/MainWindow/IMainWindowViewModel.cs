using System.Windows.Input;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
	internal interface IMainWindowViewModel : IViewModel
    {
        ICommand SwitchToPage { get; }
        MainPage SelectedPage { get; }        

        IOverviewPageViewModel          OverviewPageViewModel          { get; }
        IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        IUserPageViewModel              UserPageViewModel              { get; }
        ILicencePageViewModel           LicencePageViewModel           { get; }
        IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
		ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
		IOptionsPageViewModel           OptionsPageViewModel           { get; }
        IAboutPageViewModel             AboutPageViewModel             { get; }
    }
}
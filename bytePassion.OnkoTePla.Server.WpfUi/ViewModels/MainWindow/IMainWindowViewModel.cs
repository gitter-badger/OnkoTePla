using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : IViewModel
    {
        ICommand SwitchToPage { get; }
        MainPage SelectedPage { get; }        

        IOverviewPageViewModel       OverviewPageViewModel       { get; }
        IConnectionsPageViewModel    ConnectionsPageViewModel    { get; }
        IUserPageViewModel           UserPageViewModel           { get; }
        ILicencePageViewModel        LicencePageViewModel        { get; }
        IInfrastructurePageViewModel InfrastructurePageViewModel { get; }
        IOptionsPageViewModel        OptionsPageViewModel        { get; }
    }
}
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModelSampleData : IMainWindowViewModel
    {
        public MainWindowViewModelSampleData()
        {
            SelectedPage = 0;

            OverviewPageViewModel       = new OverviewPageViewModelSampleData();
            ConnectionsPageViewModel    = new ConnectionsPageViewModelSampleData();
            UserPageViewModel           = new UserPageViewModelSampleData();
            LicencePageViewModel        = new LicencePageViewModelSampleData();
            InfrastructurePageViewModel = new InfrastructurePageViewModelSampleData();
            OptionsPageViewModel        = new OptionsPageViewModelSampleData();
        }

        public ICommand SwitchToPage { get; } = null;
        public MainPage SelectedPage { get; }

        public IOverviewPageViewModel       OverviewPageViewModel       { get; }
        public IConnectionsPageViewModel    ConnectionsPageViewModel    { get; }
        public IUserPageViewModel           UserPageViewModel           { get; }
        public ILicencePageViewModel        LicencePageViewModel        { get; }
        public IInfrastructurePageViewModel InfrastructurePageViewModel { get; }
        public IOptionsPageViewModel        OptionsPageViewModel        { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
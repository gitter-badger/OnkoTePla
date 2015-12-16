using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
    {
        private MainPage selectedPage;

        public MainWindowViewModel(IOverviewPageViewModel overviewPageViewModel, 
                                   IConnectionsPageViewModel connectionsPageViewModel, 
                                   ILicencePageViewModel licencePageViewModel, 
                                   IInfrastructurePageViewModel infrastructurePageViewModel, 
                                   IOptionsPageViewModel optionsPageViewModel)
        {
            OverviewPageViewModel       = overviewPageViewModel;
            ConnectionsPageViewModel    = connectionsPageViewModel;
            LicencePageViewModel        = licencePageViewModel;
            InfrastructurePageViewModel = infrastructurePageViewModel;
            OptionsPageViewModel        = optionsPageViewModel;

            SwitchToPage = new ParameterrizedCommand<MainPage>(page => SelectedPage = page);
        }

        public ICommand SwitchToPage { get; }

        public MainPage SelectedPage
        {
            get { return selectedPage; }
            private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
        }

        public IOverviewPageViewModel       OverviewPageViewModel       { get; }
        public IConnectionsPageViewModel    ConnectionsPageViewModel    { get; }
        public ILicencePageViewModel        LicencePageViewModel        { get; }
        public IInfrastructurePageViewModel InfrastructurePageViewModel { get; }
        public IOptionsPageViewModel        OptionsPageViewModel        { get; }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}

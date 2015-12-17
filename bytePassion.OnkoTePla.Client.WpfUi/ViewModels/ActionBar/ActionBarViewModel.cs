using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar
{
    internal class ActionBarViewModel : ViewModel, IActionBarViewModel
    {
        public ActionBarViewModel(IConnectionStatusViewModel connectionStatusViewModel)
        {
            ConnectionStatusViewModel = connectionStatusViewModel;
        }

        public ICommand ShowOverview { get; }
        public ICommand ShowSearch   { get; }
        public ICommand ShowSettings { get; }
        public ICommand Logout       { get; }
        public ICommand ShowAbout    { get; }

        public IConnectionStatusViewModel ConnectionStatusViewModel { get; }

        protected override void CleanUp()
        {
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}

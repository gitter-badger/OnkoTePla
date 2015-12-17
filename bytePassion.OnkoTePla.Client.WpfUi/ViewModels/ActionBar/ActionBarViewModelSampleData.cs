using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar
{
    internal class ActionBarViewModelSampleData : IActionBarViewModel
    {
        public ActionBarViewModelSampleData()
        {
            ConnectionStatusViewModel = new ConnectionStatusViewModelSampleData();
        }

        public ICommand ShowOverview { get; } = null;
        public ICommand ShowSearch   { get; } = null;
        public ICommand ShowSettings { get; } = null;
        public ICommand Logout       { get; } = null;
        public ICommand ShowAbout    { get; } = null;

        public IConnectionStatusViewModel ConnectionStatusViewModel { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
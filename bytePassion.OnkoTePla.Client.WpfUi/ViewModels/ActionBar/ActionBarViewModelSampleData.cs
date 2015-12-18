using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

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
        public ICommand ShowOptions  { get; } = null;
        public ICommand Logout       { get; } = null;
        public ICommand ShowAbout    { get; } = null;

        public IConnectionStatusViewModel ConnectionStatusViewModel { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
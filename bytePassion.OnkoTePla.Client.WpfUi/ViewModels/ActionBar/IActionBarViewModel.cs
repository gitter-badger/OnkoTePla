using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ActionBar
{
    internal interface IActionBarViewModel : IViewModel
    {
        ICommand ShowOverview { get; }
        ICommand ShowSearch   { get; }
        ICommand ShowOptions { get; }
        ICommand Logout       { get; }
        ICommand ShowAbout    { get; }

        IConnectionStatusViewModel ConnectionStatusViewModel { get; }
    }
}
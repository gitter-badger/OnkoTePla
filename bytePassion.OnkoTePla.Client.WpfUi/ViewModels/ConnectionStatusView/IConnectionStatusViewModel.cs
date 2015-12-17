namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView
{
    internal interface IConnectionStatusViewModel : IViewModel
    {
        bool ConnectionIsEstablished { get; }
    }
}
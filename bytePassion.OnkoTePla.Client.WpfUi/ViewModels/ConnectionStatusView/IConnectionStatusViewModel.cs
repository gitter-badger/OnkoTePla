using bytePassion.OnkoTePla.Client.WpfUi.Enums;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView
{
	internal interface IConnectionStatusViewModel : IViewModel
    {
        ConnectionStatus ConnectionStatus { get; }
		string Text { get; }
    }
}
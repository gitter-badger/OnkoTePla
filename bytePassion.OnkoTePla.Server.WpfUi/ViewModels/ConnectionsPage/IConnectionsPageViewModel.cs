using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage.Helper;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage
{
	internal interface IConnectionsPageViewModel : IViewModel
    {
		ICommand UpdateAvailableAddresses { get; }
		
		bool IsConnectionActive   { get; set; }
		bool IsActivationPossible { get; }

		string SelectedIpAddress { get; set; }
	    string ActiveConnection  { get; }

		ObservableCollection<string> AvailableIpAddresses { get; }
		ObservableCollection<ConnectedClientDisplayData> ConnectedClients { get; }
    }
}
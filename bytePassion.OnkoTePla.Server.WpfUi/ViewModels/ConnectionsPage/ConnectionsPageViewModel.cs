using System.ComponentModel;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage
{
    internal class ConnectionsPageViewModel : ViewModel, IConnectionsPageViewModel
    {
        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}

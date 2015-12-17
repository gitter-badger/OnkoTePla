using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView
{
    internal class ConnectionStatusViewModelSampleData : IConnectionStatusViewModel
    {
        public ConnectionStatusViewModelSampleData()
        {
            ConnectionIsEstablished = true;
        }

        public bool ConnectionIsEstablished { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;        
    }
}
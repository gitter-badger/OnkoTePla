using bytePassion.Lib.Types.Communication;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.ServiceModules
{
    internal class ConnectionService
    {
        public event EventHandler<EventArgs> ConnectionStatusChanged;

        private readonly Address ServerAddress;        

        public ConnectionService(Address serverAddress)
        {
            ServerAddress = serverAddress;
            IsConnected = false;
        }

        public bool IsConnected { get; }

        public void TryConnect()
        {
            
            ConnectionStatusChanged?.Invoke(this, new EventArgs());
        }

        public void TryDisconnect()
        {

            ConnectionStatusChanged?.Invoke(this, new EventArgs());
        }
    }
}

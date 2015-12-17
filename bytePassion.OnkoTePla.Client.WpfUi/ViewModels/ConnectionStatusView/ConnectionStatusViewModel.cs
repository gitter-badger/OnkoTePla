using bytePassion.Lib.FrameworkExtensions;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ConnectionStatusView
{
    internal class ConnectionStatusViewModel : ViewModel, IConnectionStatusViewModel
    {
        private bool connectionIsEstablished;

        public ConnectionStatusViewModel()
        {
            ConnectionIsEstablished = true;
        }

        public bool ConnectionIsEstablished
        {
            get { return connectionIsEstablished; }
            private set { PropertyChanged.ChangeAndNotify(this, ref connectionIsEstablished, value); }
        }

        protected override void CleanUp()
        {            
        }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }

}

using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using NetMQ;
using NetMQ.Sockets;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        private readonly NetMQContext networkContext;
        private readonly DealerSocket receiver;

        private const string Address = @"tcp://127.0.0.1:10000";

        public MainWindowViewModel()
        {
            DoSomeThing = new Command(DoIt);
            Text = "no Text";

            networkContext = NetMQContext.Create();
            receiver = networkContext.CreateDealerSocket();

            receiver.Bind(Address);
            
        }

        private void DoIt()
        {            
            var receiveThread = new Thread(() =>
                                           {
                                               var msg = receiver.ReceiveString();

                                               Application.Current.Dispatcher.Invoke(() =>
                                                                                     {
                                                                                         Text = msg;
                                                                                     });
                                           });
            receiveThread.Start();
            Text = "readyToReceive";
        }

        private string text;
        public ICommand DoSomeThing { get; }

        public string Text
        {
            get { return text; }
            private set { PropertyChanged.ChangeAndNotify(this, ref text, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

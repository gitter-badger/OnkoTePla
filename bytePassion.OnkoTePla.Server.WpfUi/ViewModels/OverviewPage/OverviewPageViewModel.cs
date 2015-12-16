using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using NetMQ;
using NetMQ.Sockets;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
    internal class OverviewPageViewModel : ViewModel, IOverviewPageViewModel
    {
        private readonly NetMQContext networkContext;
        private readonly DealerSocket sender;

        private const string Address = @"tcp://127.0.0.1:10000";

        private string text;

        public OverviewPageViewModel()
        {
            DoSomeThing = new Command(DoIt);
            Text = "nothing Done";

            networkContext = NetMQContext.Create();            
            sender = networkContext.CreateDealerSocket();
            sender.Connect(Address);
        }

        private void DoIt()
        {
            sender.Send("first msg with zeroMQ");
            Text = "\"first msg with zeroMQ\" was sended";
        }

        public ICommand DoSomeThing { get; }

        public string Text
        {
            get { return text; }
            private set { PropertyChanged.ChangeAndNotify(this, ref text, value); }
        }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}

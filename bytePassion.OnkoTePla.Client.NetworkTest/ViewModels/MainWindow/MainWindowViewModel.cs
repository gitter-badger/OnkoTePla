using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow
{
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            DoSomeThing = new Command(DoIt);
            Text = "no Text";
        }

        private void DoIt()
        {
            Text = "testText";
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

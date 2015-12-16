using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
    internal class OverviewPageViewModel : ViewModel, IOverviewPageViewModel
    {
        private string text;

        public OverviewPageViewModel()
        {
            DoSomeThing = new Command(DoIt);
            Text = "no text";
        }

        private void DoIt()
        {
            Text = "testText";
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

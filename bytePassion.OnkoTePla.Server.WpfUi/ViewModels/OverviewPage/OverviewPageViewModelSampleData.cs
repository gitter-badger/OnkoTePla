using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
    internal class OverviewPageViewModelSampleData : IOverviewPageViewModel
    {
        public OverviewPageViewModelSampleData()
        {
            Text = "blubb";
        }

        public ICommand DoSomeThing { get; } = null;
        public string Text { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;        
    }
}
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
    {
        public MainWindowViewModelSampleData()
        {
            Text = "blubb";
        }

        public ICommand DoSomeThing { get; } = null;
        public string Text { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
using System.ComponentModel;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : INotifyPropertyChanged
    {
        ICommand DoSomeThing { get; }
        string Text { get; }
    }
}
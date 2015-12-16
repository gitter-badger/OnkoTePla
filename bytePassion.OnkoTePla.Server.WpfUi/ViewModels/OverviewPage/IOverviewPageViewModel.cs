using System.Windows.Input;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage
{
    internal interface IOverviewPageViewModel : IViewModel
    {
        ICommand DoSomeThing { get; }
        string Text { get; }
    }
}
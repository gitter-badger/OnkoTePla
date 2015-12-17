using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal interface IMainWindowViewModel : IViewModel									          
	{
		IMainViewModel MainViewModel { get; }

        bool IsMainViewVisible { get; }
	}
}

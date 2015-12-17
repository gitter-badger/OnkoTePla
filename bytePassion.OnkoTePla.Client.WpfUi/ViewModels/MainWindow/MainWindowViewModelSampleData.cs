using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.NotificationServiceContainer;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData()
		{
			MainViewModel = new MainViewModelSampleData();
		    IsMainViewVisible = true;

            NotificationServiceContainerViewModel = new NotificationServiceContainerViewModelSampleData();
		}
					            
        public IMainViewModel MainViewModel { get; }
        public bool IsMainViewVisible { get; }

        public INotificationServiceContainerViewModel NotificationServiceContainerViewModel { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

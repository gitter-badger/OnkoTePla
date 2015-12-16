using bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.NetworkTest
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            ////////                                                                             //////////
            ////////                          Composition Root and Setup                         //////////
            ////////                                                                             //////////
            ///////////////////////////////////////////////////////////////////////////////////////////////
            
            var mainWindowViewModel = new MainWindowViewModel();

            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };

            mainWindow.ShowDialog();
        }
    }
}

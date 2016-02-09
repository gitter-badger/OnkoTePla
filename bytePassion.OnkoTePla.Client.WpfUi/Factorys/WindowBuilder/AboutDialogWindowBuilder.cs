using System;
using System.Windows;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AboutDialog;
using bytePassion.OnkoTePla.Client.WpfUi.Views;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class AboutDialogWindowBuilder : IWindowBuilder<AboutDialog>
    {
        private readonly string versionNumber;

        public AboutDialogWindowBuilder(string versionNumber)
        {
            this.versionNumber = versionNumber;
        }

        public AboutDialog BuildWindow(Action<string> errorCallback)
        {
            var aboutDialogViewModel = new AboutDialogViewModel(versionNumber);

            return new AboutDialog
                   {
                       Owner = Application.Current.MainWindow,
                       DataContext = aboutDialogViewModel
                   };
        }

        public void DisposeWindow(AboutDialog buildedWindow)
        {            
        }
    }
}

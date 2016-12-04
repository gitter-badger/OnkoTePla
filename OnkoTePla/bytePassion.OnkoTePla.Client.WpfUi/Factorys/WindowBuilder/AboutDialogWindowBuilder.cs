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
		private readonly Action<string> errorCallback;

		public AboutDialogWindowBuilder(string versionNumber, Action<string> errorCallback)
		{
			this.versionNumber = versionNumber;
			this.errorCallback = errorCallback;
		}

		public AboutDialog BuildWindow()
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
			throw new NotImplementedException();
		}
    }
}

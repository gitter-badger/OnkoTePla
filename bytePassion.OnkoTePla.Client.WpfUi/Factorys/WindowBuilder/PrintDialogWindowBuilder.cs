using System;
using System.Windows;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintDialog;
using bytePassion.OnkoTePla.Client.WpfUi.Views;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class PrintDialogWindowBuilder : IWindowBuilder<PrintDialog>
	{					
		public PrintDialog BuildWindow()
		{           
			return new PrintDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new PrintDialogViewModel()
			       };
		}

		public void DisposeWindow(PrintDialog buildedWindow)
		{
			throw new NotImplementedException();
		}		
	}
}

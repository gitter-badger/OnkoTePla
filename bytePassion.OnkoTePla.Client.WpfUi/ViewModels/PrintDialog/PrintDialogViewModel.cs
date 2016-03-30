using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.Commands;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PrintDialog
{
	internal class PrintDialogViewModel : ViewModel, IPrintDialogViewModel
	{
		public PrintDialogViewModel()
		{
			Cancel = new Command(CloseWindow);
			Print  = new Command(DoPrint);
		}

		public ICommand Cancel { get; }
		public ICommand Print  { get; }

		private void DoPrint()
		{
			
		}

		private static void CloseWindow ()
		{
			var windows = Application.Current.Windows
											 .OfType<Views.PrintDialog>()
											 .ToList();

			if (windows.Count == 1)
				windows[0].Close();
			else
				throw new Exception("inner error");
		}

		protected override void CleanUp() {	}
		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}

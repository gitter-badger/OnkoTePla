using System;
using System.Windows;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WPFVisualization.Views;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.WindowBuilder
{
	public class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		public AddAppointmentDialog BuildWindow()
		{
			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel()
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Windows;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.Views;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.WindowBuilder
{
	public class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IDataCenter dataCenter;

		public AddAppointmentDialogWindowBuilder(IDataCenter dataCenter)
		{
			this.dataCenter = dataCenter;
		}

		public AddAppointmentDialog BuildWindow()
		{
			IPatientSelectorViewModel patientSelectorViewModel = new PatientSelectorViewModel(dataCenter);
			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel(patientSelectorViewModel)
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

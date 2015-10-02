using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModel : IAddAppointmentDialogViewModel
	{

		public AddAppointmentDialogViewModel(IPatientSelectorViewModel patientSelectorViewModel)
		{
			PatientSelectorViewModel = patientSelectorViewModel;

			CloseDialog = new Command(() =>
			{
				var windows = Application.Current.Windows
												 .OfType<Views.AddAppointmentDialog>()
												 .ToList();

				if (windows.Count() == 1)
					windows[0].Close();
					
			});
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }
		public ICommand CloseDialog { get; }
	}
}

using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public interface IAddAppointmentDialogViewModel
	{
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		ICommand CloseDialog { get; }

	}
}
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModelSampleData : IAddAppointmentDialogViewModel
	{
		public AddAppointmentDialogViewModelSampleData()
		{
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }
		public ICommand CloseDialog { get; } = null;
	}
}
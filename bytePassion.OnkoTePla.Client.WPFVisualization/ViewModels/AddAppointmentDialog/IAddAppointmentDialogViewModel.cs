using System;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public interface IAddAppointmentDialogViewModel : IDisposable, 
													  IViewModel
	{
		IPatientSelectorViewModel PatientSelectorViewModel { get; }

		ICommand CloseDialog { get; }

		Patient SelectedPatient { get; }
	}
}
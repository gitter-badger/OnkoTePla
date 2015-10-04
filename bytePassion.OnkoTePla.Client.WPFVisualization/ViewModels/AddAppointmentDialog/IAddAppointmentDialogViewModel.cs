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
		ICommand CreateAppointment { get; }

		ICommand HourPlusOne        { get; }
		ICommand HourMinusOne       { get; }
		ICommand MinutePlusFifteen  { get; }
		ICommand MinuteMinusFifteen { get; }

		byte DurationHours   { get; }
		byte DurationMinutes { get; }

		Patient SelectedPatient { get; }
		string Description { set; }
	}
}
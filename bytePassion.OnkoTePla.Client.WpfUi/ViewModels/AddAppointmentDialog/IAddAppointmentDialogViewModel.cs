using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Patients;
using System;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog
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

		AppointmentCreationState CreationState { get; }

		Patient SelectedPatient { get; }
		string Description { set; }
	}
}
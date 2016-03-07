using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Patients;
using System;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog
{
    internal class AddAppointmentDialogViewModelSampleData : IAddAppointmentDialogViewModel
	{
		public AddAppointmentDialogViewModelSampleData()
		{
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();

			SelectedPatient = new Patient("John Doh", new Date(12,4,1978), true, new Guid(), "externalID");

			DurationHours = 2;
			DurationMinutes = 15;

			CreationState = AppointmentCreationState.NoPatientSelected;
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }

		public ICommand CloseDialog        { get; } = null;
		public ICommand CreateAppointment  { get; } = null;
		
		public ICommand HourPlusOne        { get; } = null;
		public ICommand HourMinusOne       { get; } = null;
		public ICommand MinutePlusFifteen  { get; } = null;
		public ICommand MinuteMinusFifteen { get; } = null;

		public byte DurationHours   { get; }
		public byte DurationMinutes { get; }

		public AppointmentCreationState CreationState { get; }

		public Patient SelectedPatient { get; }
		public string  Description     { set {} }

		public void Dispose() { }
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
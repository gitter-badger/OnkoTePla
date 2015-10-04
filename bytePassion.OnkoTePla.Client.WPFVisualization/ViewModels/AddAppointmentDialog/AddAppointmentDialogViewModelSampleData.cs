using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Patients;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModelSampleData : IAddAppointmentDialogViewModel
	{
		public AddAppointmentDialogViewModelSampleData()
		{
			PatientSelectorViewModel = new PatientSelectorViewModelSampleData();

			SelectedPatient = new Patient("John Doh", new Date(12,4,1978), true, new Guid(), "externalID");
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }
		public ICommand CloseDialog { get; } = null;
		public Patient SelectedPatient { get; }

		public void Dispose() { }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
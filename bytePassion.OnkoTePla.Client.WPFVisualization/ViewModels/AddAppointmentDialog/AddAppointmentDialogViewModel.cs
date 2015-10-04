using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModel : DisposingObject,
												 IAddAppointmentDialogViewModel
	{
		private readonly IGlobalState<Patient> selectedPatientVariable;

		private Patient selectedPatient;

		public AddAppointmentDialogViewModel(IPatientSelectorViewModel patientSelectorViewModel,
											 IViewModelCommunication viewModelCommunication)
		{

			selectedPatientVariable = viewModelCommunication.GetGlobalViewModelVariable<Patient>(
				Constants.SelectedPatientVariable
			);

			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			CloseDialog = new Command(() =>
			{
				var windows = Application.Current.Windows
												 .OfType<Views.AddAppointmentDialog>()
												 .ToList();

				if (windows.Count() == 1)
					windows[0].Close();
				else
					throw new Exception("inner error");
					
			});

			SelectedPatient = Patient.Dummy;
		}

		private void OnSelectedPatientVariableChanged(Patient patient)
		{
			SelectedPatient = patient ?? Patient.Dummy;
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }
		public ICommand CloseDialog { get; }

		public Patient SelectedPatient
		{
			get { return selectedPatient; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
		}

		public override void CleanUp()
		{
			selectedPatientVariable.StateChanged -= OnSelectedPatientVariableChanged;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

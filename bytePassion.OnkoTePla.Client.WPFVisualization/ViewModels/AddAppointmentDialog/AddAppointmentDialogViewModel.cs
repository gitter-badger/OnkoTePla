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
		private string description;

		private byte durationMinutes;
		private byte durationHours;

		public AddAppointmentDialogViewModel(IPatientSelectorViewModel patientSelectorViewModel,
											 IViewModelCommunication viewModelCommunication)
		{

			selectedPatientVariable = viewModelCommunication.GetGlobalViewModelVariable<Patient>(
				Constants.SelectedPatientVariable
			);

			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			CloseDialog       = new Command(CloseWindow);
			CreateAppointment = new Command(CreateNewAppointment);

			HourPlusOne        = new Command(DoHourPlusOne,
											 CanHourPlusOne,
											 new UpdateCommandInformation(this, nameof(DurationHours)));
			HourMinusOne       = new Command(DoHourMinusOne,
											 CanHourMinusOne,
											 new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));
			MinutePlusFifteen  = new Command(DoMinutePlusFifteen,
											 CanMinutePlusFifteen,
											 new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));
			MinuteMinusFifteen = new Command(DoMinuteMinusFifteen,
											 CanMinuteMinusFifteen,
											 new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));

			SelectedPatient = Patient.Dummy;

			DurationMinutes = 0;
			DurationHours = 2;
		}

		private bool CanHourPlusOne()
		{
			return DurationHours < 7;
		}

		private bool CanHourMinusOne()
		{
			if (DurationHours > 1)
				return true;

			if (DurationHours == 1)
			{
				return DurationMinutes > 0;
			}

			return false;
		}

		private bool CanMinuteMinusFifteen()
		{
			if (DurationMinutes > 15)
				return true;

			return DurationHours > 0;
		}

		private bool CanMinutePlusFifteen()
		{
			if (DurationMinutes < 45)
				return true;

			return CanHourPlusOne();
		}


		private void OnSelectedPatientVariableChanged(Patient patient)
		{
			SelectedPatient = patient ?? Patient.Dummy;
		}

		private void CloseWindow()
		{
			var windows = Application.Current.Windows
												 .OfType<Views.AddAppointmentDialog>()
												 .ToList();

			if (windows.Count() == 1)
				windows[0].Close();
			else
				throw new Exception("inner error");
		}

		private void CreateNewAppointment ()
		{
			// TODO
		}

		private void DoHourPlusOne()
		{
			DurationHours += 1;
		}

		private void DoHourMinusOne()
		{
			DurationHours -= 1;
		}

		private void DoMinutePlusFifteen()
		{
			var newMinutes = (byte)(DurationMinutes + 15);

			if (newMinutes == 60)
			{
				DurationMinutes = 0;
				DoHourPlusOne();
			}
			else
			{
				DurationMinutes = newMinutes;
			}
		}

		private void DoMinuteMinusFifteen()
		{
			var newMinuts = DurationMinutes - 15;

			if (newMinuts < 0)
			{
				DurationMinutes = 45;
				DoHourMinusOne();
			}
			else
			{
				DurationMinutes = (byte) newMinuts;
			}
		}

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }
		public ICommand CloseDialog { get; }
		public ICommand CreateAppointment { get; }
		public ICommand HourPlusOne { get; }
		public ICommand HourMinusOne { get; }
		public ICommand MinutePlusFifteen { get; }
		public ICommand MinuteMinusFifteen { get; }

		public byte DurationHours
		{
			get { return durationHours; }
			private set { PropertyChanged.ChangeAndNotify(this, ref durationHours, value); }
		}

		public byte DurationMinutes
		{
			get { return durationMinutes; }
			private set { PropertyChanged.ChangeAndNotify(this, ref durationMinutes, value); }
		}

		public Patient SelectedPatient
		{
			get { return selectedPatient; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
		}

		public string Description
		{
			set { description = value; }
		}

		public override void CleanUp()
		{
			selectedPatientVariable.StateChanged -= OnSelectedPatientVariableChanged;

			((Command)HourPlusOne).Dispose();
			((Command)HourMinusOne).Dispose();
			((Command)MinutePlusFifteen).Dispose();
			((Command)MinuteMinusFifteen).Dispose();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

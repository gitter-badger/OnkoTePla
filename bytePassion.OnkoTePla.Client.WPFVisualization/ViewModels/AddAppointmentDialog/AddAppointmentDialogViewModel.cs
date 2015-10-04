using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using Duration = bytePassion.Lib.TimeLib.Duration;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog
{
	public class AddAppointmentDialogViewModel : DisposingObject,
		IAddAppointmentDialogViewModel
	{
		private readonly IViewModelCommunication superViewModelCommunication;
		private readonly IDataCenter dataCenter;
		private readonly Date creationDate;
		private readonly Guid medicalPracticeId;
		private readonly IGlobalState<Patient> selectedPatientVariable;
		private readonly IDictionary<TherapyPlaceRowIdentifier, IEnumerable<TimeSlot>> allAvailableTimeSlots;

		private Patient selectedPatient;

		private byte durationMinutes;
		private byte durationHours;
		private AppointmentCreationState creationState;
		private string description;
		
		private Tuple<TherapyPlaceRowIdentifier, TimeSlot> firstFittingTimeSlot; 

		public AddAppointmentDialogViewModel(IPatientSelectorViewModel patientSelectorViewModel,
											 IViewModelCommunication subViewModelCommunication,
											 IViewModelCommunication superViewModelCommunication,
											 IDataCenter dataCenter,
											 Date creationDate,
											 Guid medicalPracticeId)
		{
			this.superViewModelCommunication = superViewModelCommunication;
			this.dataCenter = dataCenter;
			this.creationDate = creationDate;
			this.medicalPracticeId = medicalPracticeId;

			allAvailableTimeSlots = GetFreeTimeSlotsForADay(creationDate, medicalPracticeId);		

			selectedPatientVariable = subViewModelCommunication.GetGlobalViewModelVariable<Patient>(
				Constants.SelectedPatientVariable
			);

			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			CloseDialog = new Command(CloseWindow);
			CreateAppointment = new Command(CreateNewAppointment,
											() => CreationState != AppointmentCreationState.NoPatientSelected && 
												  CreationState != AppointmentCreationState.NoSpaceAvailable,
											new UpdateCommandInformation(this, nameof(CreationState)));

			HourPlusOne = new Command(DoHourPlusOne,
									  CanHourPlusOne,
									  new UpdateCommandInformation(this, nameof(DurationHours)));

			HourMinusOne = new Command(DoHourMinusOne,
									   CanHourMinusOne,
									   new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));

			MinutePlusFifteen = new Command(DoMinutePlusFifteen,
											CanMinutePlusFifteen,
											new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));

			MinuteMinusFifteen = new Command(DoMinuteMinusFifteen,
											 CanMinuteMinusFifteen,
											 new UpdateCommandInformation(this, nameof(DurationHours), nameof(DurationMinutes)));

			SelectedPatient = Patient.Dummy;

			DurationMinutes = 0;
			DurationHours = 2;

			CreationState = AppointmentCreationState.NoPatientSelected;
		}

		private void DetermineCreationState()
		{
			firstFittingTimeSlot = ComputeFirstFittingSlot();

			if (firstFittingTimeSlot == null)
			{
				CreationState = AppointmentCreationState.NoSpaceAvailable;
			}
			else if (SelectedPatient != Patient.Dummy)
			{
				if (string.IsNullOrWhiteSpace(Description))
					CreationState = AppointmentCreationState.PatientSelected;
				else
					CreationState = AppointmentCreationState.PatientAndDespriptionAvailable;
			}
			else
				CreationState = AppointmentCreationState.NoPatientSelected;

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

		private void CreateNewAppointment()
		{
			var therapyPlace = dataCenter.GetMedicalPracticeByDateAndId(creationDate, medicalPracticeId)
										 .GetTherapyPlaceById(firstFittingTimeSlot.Item1.TherapyPlaceId);

			var duration = new Duration((uint) (DurationHours * 3600 + DurationMinutes * 60));

			var newAppointmentViewModel = new AppointmentViewModel(new Appointment(SelectedPatient, 
								    	                           				   Description, 
								    	                           				   therapyPlace, 
								    	                           				   creationDate, 
								    	                           				   firstFittingTimeSlot.Item2.Begin,
								    	                           				   firstFittingTimeSlot.Item2.Begin + duration,
								    	                           				   Guid.NewGuid()), 
								    	                           superViewModelCommunication, 
								    	                           dataCenter,
								    	                           new TherapyPlaceRowIdentifier(new AggregateIdentifier(creationDate, medicalPracticeId),
								    	                           							     firstFittingTimeSlot.Item1.TherapyPlaceId));

			newAppointmentViewModel.SwitchToEditMode.Execute(true);
			CloseWindow();
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
			var newMinutes = (byte) (DurationMinutes + 15);

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

		public ICommand CloseDialog       { get; }
		public ICommand CreateAppointment { get; }
		 
		public ICommand HourPlusOne        { get; }
		public ICommand HourMinusOne       { get; }
		public ICommand MinutePlusFifteen  { get; }
		public ICommand MinuteMinusFifteen { get; }

		public byte DurationHours
		{
			get { return durationHours; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref durationHours, value);
				DetermineCreationState();
			}
		}

		public byte DurationMinutes
		{
			get { return durationMinutes; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref durationMinutes, value);
				DetermineCreationState();
			}
		}

		public AppointmentCreationState CreationState
		{
			get { return creationState; }
			private set { PropertyChanged.ChangeAndNotify(this, ref creationState, value); }
		}

		public Patient SelectedPatient
		{
			get { return selectedPatient; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value);
				DetermineCreationState();
			}
		}

		public string Description
		{
			set
			{
				description = value;
				DetermineCreationState();
			}
			get { return description; }
		}

		public override void CleanUp()
		{
			selectedPatientVariable.StateChanged -= OnSelectedPatientVariableChanged;

			((Command) HourPlusOne).Dispose();
			((Command) HourMinusOne).Dispose();
			((Command) MinutePlusFifteen).Dispose();
			((Command) MinuteMinusFifteen).Dispose();
		}

		public event PropertyChangedEventHandler PropertyChanged;


		private IDictionary<TherapyPlaceRowIdentifier, IEnumerable<TimeSlot>> GetFreeTimeSlotsForADay(Date date, Guid medicalPracticeId)
		{

			IDictionary<TherapyPlaceRowIdentifier, IEnumerable<TimeSlot>> allSlots =
				new Dictionary<TherapyPlaceRowIdentifier, IEnumerable<TimeSlot>>();

			var newMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(date, medicalPracticeId);

			if (newMedicalPracticeVersion.HoursOfOpening.IsOpen(date))
			{
				var readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(
					new AggregateIdentifier(date, medicalPracticeId)
					);

				IDictionary<TherapyPlace, IList<Appointment>> sortedAppointments =
					new Dictionary<TherapyPlace, IList<Appointment>>();

				foreach (var therapyPlace in newMedicalPracticeVersion.GetAllTherapyPlaces())
					sortedAppointments.Add(therapyPlace, new List<Appointment>());

				readModel.Appointments.Do(appointment => sortedAppointments[appointment.TherapyPlace].Add(appointment));

				var openingTime = newMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(date);
				var closingTime = newMedicalPracticeVersion.HoursOfOpening.GetClosingTime(date);

				foreach (var therapyRowData in sortedAppointments)
				{
					var slots = ComputeSlots(openingTime, closingTime, therapyRowData.Value);

					allSlots.Add(new TherapyPlaceRowIdentifier(new AggregateIdentifier(date, medicalPracticeId), therapyRowData.Key.Id),
						         slots);
				}
			}

			return allSlots;
		}

		private static IEnumerable<TimeSlot> ComputeSlots(Time openingTime, Time closingTime,
			IEnumerable<Appointment> appointments)
		{
			var sortedAppointments = appointments.ToList();
			sortedAppointments.Sort((appointment, appointment1) => appointment.StartTime.CompareTo(appointment1.StartTime));

			var startOfSlots = new List<Time>();
			var endOfSlots = new List<Time>();

			startOfSlots.Add(openingTime);

			foreach (var appointment in sortedAppointments)
			{
				endOfSlots.Add(appointment.StartTime);
				startOfSlots.Add(appointment.EndTime);
			}

			endOfSlots.Add(closingTime);

			var slots = new List<TimeSlot>();

			for (int i = 0; i < startOfSlots.Count; i++)
			{
				slots.Add(new TimeSlot(startOfSlots[i], endOfSlots[i]));
			}

			return slots;
		}

		private Tuple<TherapyPlaceRowIdentifier, TimeSlot> ComputeFirstFittingSlot ()
		{
			var duration = new Duration((uint) (DurationHours * 3600 + DurationMinutes * 60));

			foreach (var timeSlotsOfARow in allAvailableTimeSlots)
			{
				foreach (var timeSlot in timeSlotsOfARow.Value)
				{
					var slotDuration = new Duration(timeSlot.Begin, timeSlot.End);
					if (slotDuration >= duration)
						return new Tuple<TherapyPlaceRowIdentifier, TimeSlot>(
							new TherapyPlaceRowIdentifier(new AggregateIdentifier(creationDate, medicalPracticeId), timeSlotsOfARow.Key.TherapyPlaceId),
							timeSlot 
						);
				}
			}

			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using Duration = bytePassion.Lib.TimeLib.Duration;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper
{
	public class AppointmentModifications : DisposingObject, INotifyPropertyChanged
	{		
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IGlobalState<Date> selectedDateVariable; 

		
		private Time beginTime;
		private Time endTime;
		private Time lastSetBeginTime;
		private Time lastSetEndTime;

		private Time currentDayOpeningTime;
		private Time currentDayClosingTime;

		private Time currentSlotBegin;
		private Time currentSlotEnd;

		private MedicalPractice currentMedicalPracticeVersion;
		private double currentGridWidth;		
		private bool hideAppointment;
		private TherapyPlaceRowIdentifier currentLocation;

		public AppointmentModifications(Appointment appointment,
										Guid medicalPracticeId, 
										IDataCenter dataCenter, 
										IViewModelCommunication viewModelCommunication)
		{
			Appointment = appointment;			
			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;								
						
			var aggregateIdentifier = new AggregateIdentifier(appointment.Day, medicalPracticeId);
			var initalLocation = new TherapyPlaceRowIdentifier(aggregateIdentifier, appointment.TherapyPlace.Id);
			
			
			currentMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(initalLocation.PlaceAndDate.Date,
																					 initalLocation.PlaceAndDate.MedicalPracticeId);
			
			currentLocation = initalLocation;

			beginTime = appointment.StartTime;
			endTime   = appointment.EndTime;

			lastSetBeginTime = BeginTime;
			lastSetEndTime   = EndTime;

			selectedDateVariable = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);

			selectedDateVariable.StateChanged += OnSelectedDateVariableChanged;

			ComputeSlotInformations();
		}

		private void OnSelectedDateVariableChanged(Date date)
		{
			if (date != CurrentLocation.PlaceAndDate.Date)
			{

				var newMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(date, currentMedicalPracticeVersion.Id);

				if (newMedicalPracticeVersion.HoursOfOpening.IsOpen(date))
				{
					var readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(
						new AggregateIdentifier(date, CurrentLocation.PlaceAndDate.MedicalPracticeId)
						);

					IDictionary<TherapyPlace, IList<Appointment>> sortedAppointments =
						new Dictionary<TherapyPlace, IList<Appointment>>();

					foreach (var therapyPlace in newMedicalPracticeVersion.GetAllTherapyPlaces())
						sortedAppointments.Add(therapyPlace, new List<Appointment>());

					foreach (var appointment in readModel.Appointments)
						if (appointment != Appointment)
							sortedAppointments[appointment.TherapyPlace].Add(appointment);

					var openingTime = newMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(date);
					var closingTime = newMedicalPracticeVersion.HoursOfOpening.GetClosingTime(date);

					var appointmentDuration = Time.GetDurationBetween(BeginTime, EndTime);

					foreach (var therapyRowData in sortedAppointments)
					{
						var slots = ComputeSlots(openingTime, closingTime, therapyRowData.Value);
						var suitableSlot = GetSlotForAppointment(slots, appointmentDuration);

						if (suitableSlot != null)
						{
							SetNewLocation(
								new TherapyPlaceRowIdentifier(new AggregateIdentifier(date, 
																					  CurrentLocation.PlaceAndDate.MedicalPracticeId),
								                              therapyRowData.Key.Id), 
								suitableSlot.Begin,
								suitableSlot.Begin + appointmentDuration
							);
							return;
						}
					}

					viewModelCommunication.Send(
						new ShowNotification("cannot move the appointment to that day. To timeslot is big enough!")
					);
				}
				else
				{
					viewModelCommunication.Send(
						new ShowNotification("cannot move an appointment to a day where the practice is closed!")
					);					
				}

				selectedDateVariable.Value = CurrentLocation.PlaceAndDate.Date;
			}
		}

		private static TimeSlot GetSlotForAppointment(IEnumerable<TimeSlot> timeSlots, Duration appointmentDuration)
		{
			return timeSlots.FirstOrDefault(slot => Time.GetDurationBetween(slot.Begin, slot.End) >= appointmentDuration);
		}

		private IEnumerable<TimeSlot> ComputeSlots (Time openingTime, Time closingTime, IEnumerable<Appointment> appointments)
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

		public Appointment Appointment { get; }

		public Time BeginTime
		{
			get { return beginTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref beginTime, value); }
		}

		public Time EndTime
		{
			get { return endTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref endTime, value); }
		}

		public bool ShowDisabledOverlay
		{
			get { return hideAppointment; }
			set { PropertyChanged.ChangeAndNotify(this, ref hideAppointment, value); }
		}

		public TherapyPlaceRowIdentifier CurrentLocation
		{
			get { return currentLocation; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentLocation, value); }
		}

		public void SetNewLocation(TherapyPlaceRowIdentifier newLocation, Time newBeginTime, Time newEndTime)
		{
			if (CurrentLocation == null ||
			   (CurrentLocation != null && CurrentLocation.PlaceAndDate != newLocation.PlaceAndDate))
			{
				currentMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(newLocation.PlaceAndDate.Date,
				                                                                         newLocation.PlaceAndDate.MedicalPracticeId);
			}

			CurrentLocation = newLocation;
			
			var duration = Time.GetDurationBetween(newBeginTime, newEndTime);
			var beginTimeToSnap = GetTimeToSnap(newBeginTime);
			lastSetBeginTime = beginTimeToSnap;
			BeginTime = beginTimeToSnap;


			var endTimeToSnap = GetTimeToSnap(beginTimeToSnap + duration);
			lastSetEndTime = endTimeToSnap;
			EndTime = endTimeToSnap;

			ComputeSlotInformations();
		}	
		
		public void SetNewTimeShiftDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (Time.GetDurationBetween(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);


			if (deltaInPixel > 0)
			{
				var tempEndTime = CheckEndTime(deltaInPixel > 0 ? lastSetEndTime + durationDelta : lastSetEndTime - durationDelta);
				var actualTimeDelta = Time.GetDurationBetween(lastSetEndTime, tempEndTime);
				
				BeginTime = lastSetBeginTime + actualTimeDelta;
				EndTime   = lastSetEndTime + actualTimeDelta;
			}
			else
			{
				var tmpBeginTime = CheckBeginTime(deltaInPixel > 0 ? lastSetBeginTime + durationDelta : lastSetBeginTime - durationDelta);
				var actualTimeDelta = Time.GetDurationBetween(lastSetBeginTime, tmpBeginTime);
				
				BeginTime = lastSetBeginTime - actualTimeDelta;
				EndTime   = lastSetEndTime - actualTimeDelta;
			}						
		}

		public void FixTimeShiftDelta()
		{
			var duration = Time.GetDurationBetween(BeginTime, EndTime);
			var beginTimeToSnap = GetTimeToSnap(BeginTime);
			lastSetBeginTime = beginTimeToSnap;
			BeginTime = beginTimeToSnap;


			var endTimeToSnap = GetTimeToSnap(beginTimeToSnap + duration);
			lastSetEndTime = endTimeToSnap;
			EndTime = endTimeToSnap;
		}

		public void SetNewEndTimeDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (Time.GetDurationBetween(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);

			EndTime = CheckEndTime(deltaInPixel > 0 ? lastSetEndTime + durationDelta : lastSetEndTime - durationDelta);
		}

		private Time CheckEndTime(Time endTimeToCheck)
		{
			if (endTimeToCheck > currentSlotEnd)
				return currentSlotEnd;

			var minimalTimeEnd = BeginTime + new Duration(60*15);

			if (endTimeToCheck < minimalTimeEnd)
				return minimalTimeEnd;
						
			return endTimeToCheck;
		}

		public void FixEndTimeDelta()
		{
			var timeToSnap = GetTimeToSnap(EndTime);
			lastSetEndTime = timeToSnap;
			EndTime = timeToSnap;
		}

		public void SetNewBeginTimeDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (Time.GetDurationBetween(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);						
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);

			BeginTime = CheckBeginTime(deltaInPixel > 0 ? lastSetBeginTime + durationDelta : lastSetBeginTime - durationDelta);			
		}

		private Time CheckBeginTime(Time beginTimeToCheck)
		{
			if (beginTimeToCheck < currentSlotBegin)
				return currentSlotBegin;

			var minimalBeginTime = EndTime - new Duration(60*15);

			if (beginTimeToCheck > minimalBeginTime)
				return minimalBeginTime;

			return beginTimeToCheck;
		}

		public void FixBeginTimeDelta()
		{
			var timeToSnap = GetTimeToSnap(BeginTime);
			lastSetBeginTime = timeToSnap;
			BeginTime = timeToSnap;			
		}

		private static Time GetTimeToSnap(Time time)
		{
			var m = time.Minute;
			
			if (          m <=  7) return new Time(time.Hour,  0);
			if (m >  7 && m <= 22) return new Time(time.Hour, 15);
			if (m > 22 && m <= 37) return new Time(time.Hour, 30);
			if (m > 37 && m <= 52) return new Time(time.Hour, 45);
			if (m > 52 && m <= 59) return new Time((byte)(time.Hour+1), 0);

			throw new Exception("internal Error");
		}

		private void ComputeSlotInformations()
		{
			currentDayOpeningTime = currentMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(CurrentLocation.PlaceAndDate.Date);
			currentDayClosingTime = currentMedicalPracticeVersion.HoursOfOpening.GetClosingTime(CurrentLocation.PlaceAndDate.Date);
			currentGridWidth = viewModelCommunication.GetGlobalViewModelVariable<Size>(AppointmentGridSizeVariable)
													 .Value
													 .Width;

			var currentReadModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(CurrentLocation.PlaceAndDate);

			var appointmentWithCorrectStartAndEnd = new Appointment(Appointment.Patient, 
																	Appointment.Description,
																	Appointment.TherapyPlace,
																	Appointment.Day, 
																	BeginTime, 
																	EndTime, 
																	Appointment.Id);

			var appointmentsWithinTheSameRow = currentReadModel.Appointments
															   .Where(appointment => appointment.TherapyPlace.Id == CurrentLocation.TherapyPlaceId)
															   .Where(appointment => appointment.Id != Appointment.Id)
															   .Append(appointmentWithCorrectStartAndEnd)
															   .ToList();

			appointmentsWithinTheSameRow.Sort((appointment, appointment1) => appointment.StartTime.CompareTo(appointment1.StartTime));
			var indexOfThisAppointment = appointmentsWithinTheSameRow.IndexOf(appointmentWithCorrectStartAndEnd);

			if (appointmentsWithinTheSameRow.Count == 1)
			{
				currentSlotBegin = currentDayOpeningTime;
				currentSlotEnd   = currentDayClosingTime;
			}
			else if (indexOfThisAppointment == 0)
			{
				currentSlotBegin = currentDayOpeningTime;
				currentSlotEnd   = appointmentsWithinTheSameRow[indexOfThisAppointment + 1].StartTime;
			}
			else if (indexOfThisAppointment == appointmentsWithinTheSameRow.Count - 1)
			{
				currentSlotBegin = appointmentsWithinTheSameRow[indexOfThisAppointment - 1].EndTime;
				currentSlotEnd   = currentDayClosingTime;
			}
			else
			{
				currentSlotBegin = appointmentsWithinTheSameRow[indexOfThisAppointment - 1].EndTime;
				currentSlotEnd   = appointmentsWithinTheSameRow[indexOfThisAppointment + 1].StartTime;
			}
		}
		
		public override void CleanUp()
		{
			selectedDateVariable.StateChanged -= OnSelectedDateVariableChanged;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

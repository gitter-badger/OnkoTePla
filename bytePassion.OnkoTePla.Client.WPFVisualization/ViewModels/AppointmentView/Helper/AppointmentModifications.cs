using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using Duration = bytePassion.Lib.TimeLib.Duration;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper
{
	public class AppointmentModifications : INotifyPropertyChanged
	{		
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;

		
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

			ComputeSlotInformations();
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

			BeginTime = newBeginTime;
			EndTime   = newEndTime;

			lastSetBeginTime = BeginTime;
			lastSetEndTime   = EndTime;

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
			lastSetBeginTime = BeginTime;
			lastSetEndTime = EndTime;
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
			lastSetEndTime = EndTime;
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
			lastSetBeginTime = BeginTime;
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

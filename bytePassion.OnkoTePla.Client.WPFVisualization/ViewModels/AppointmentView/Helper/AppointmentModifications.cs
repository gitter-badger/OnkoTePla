using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using Duration = bytePassion.Lib.TimeLib.Duration;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper
{
	public class AppointmentModifications : INotifyPropertyChanged
	{
		private readonly Guid medicalPracticeId;
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

		private AppointmentsOfADayReadModel currentReadModel;

		private Date currentDate;
		private double currentGridWidth;
		private MedicalPractice currentMedicalPracticeVersion;		

		public AppointmentModifications(Appointment appointment,
										Guid medicalPracticeId, 
										IDataCenter dataCenter, 
										IViewModelCommunication viewModelCommunication)
		{
			Appointment = appointment;
			this.medicalPracticeId = medicalPracticeId;
			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;

			currentDate = Appointment.Day;
			currentMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(currentDate, medicalPracticeId);

			currentDayOpeningTime = Time.Dummy;
			currentDayClosingTime = Time.Dummy;
			currentSlotBegin = Time.Dummy;
			currentSlotEnd = Time.Dummy;

			currentReadModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(new AggregateIdentifier(appointment.Day,
																													 medicalPracticeId));

			beginTime = appointment.StartTime;
			lastSetBeginTime = beginTime;
			endTime = appointment.EndTime;
			lastSetEndTime = endTime;
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

		public void SetNewEndTimeDelta(double deltaInPixel)
		{
			if (Time.IsDummy(currentDayOpeningTime))
			{
				ComputeSlotInformations();
			}

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
			if (Time.IsDummy(currentDayOpeningTime))
			{
				ComputeSlotInformations();
			}

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
			currentDayOpeningTime = currentMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(currentDate);
			currentDayClosingTime = currentMedicalPracticeVersion.HoursOfOpening.GetClosingTime(currentDate);
			currentGridWidth = viewModelCommunication.GetGlobalViewModelVariable<Size>(AppointmentGridSizeVariable)
													 .Value
													 .Width;

			var appointmentsWithinTheSameRow = currentReadModel.Appointments
																   .Where(appointment => appointment.TherapyPlace == Appointment.TherapyPlace)
																   .ToList();
			appointmentsWithinTheSameRow.Sort((appointment, appointment1) => appointment.StartTime.CompareTo(appointment1.StartTime));
			var indexOfThisAppointment = appointmentsWithinTheSameRow.IndexOf(Appointment);

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

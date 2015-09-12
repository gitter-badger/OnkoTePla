using System;
using System.ComponentModel;
using System.Windows;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
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
		private Time lastSetBeginTime;

		private Time currentTimeSlotBegin;
		private Time currentTimeSlotEnd;
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

			currentTimeSlotBegin = Time.Dummy;
			currentTimeSlotEnd = Time.Dummy;

			beginTime = appointment.StartTime;
			lastSetBeginTime = beginTime;
		}

		public Appointment Appointment { get; }

		public Time BeginTime
		{
			get { return beginTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref beginTime, value); }
		}

		public void SetNewBeginTimeDelta(double deltaInPixel)
		{
			if (Time.IsDummy(currentTimeSlotBegin))
			{
				currentTimeSlotBegin = currentMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(currentDate);
				currentTimeSlotEnd   = currentMedicalPracticeVersion.HoursOfOpening.GetClosingTime(currentDate);
				currentGridWidth = viewModelCommunication.GetGlobalViewModelVariable<Size>(AppointmentGridSizeVariable)
														 .Value
														 .Width;
			}

			var lengthOfOneHour = currentGridWidth / (Time.GetDurationBetween(currentTimeSlotEnd, currentTimeSlotBegin).Seconds / 3600.0);						
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);

			BeginTime = deltaInPixel > 0 ? lastSetBeginTime + durationDelta : lastSetBeginTime - durationDelta;			
		}

		public void FixBeginTimeDelta()
		{
			lastSetBeginTime = BeginTime;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

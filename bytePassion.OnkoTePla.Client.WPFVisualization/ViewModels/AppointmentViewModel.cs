using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentViewModel : IAppointmentViewModel
	{
		private readonly Appointment appointment;
		private ITherapyPlaceRowViewModel containerViewModel;

		private double canvasPosition;
		private double viewElementLength;

		public AppointmentViewModel(Appointment appointment, ITherapyPlaceRowViewModel containerViewModel)
		{
			this.appointment = appointment;
			this.containerViewModel = containerViewModel;

			containerViewModel.PropertyChanged += OnContainerChanged;
		}

		private void OnContainerChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var container = (ITherapyPlaceRowViewModel) sender;

			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointment.StartTime, container.TimeSlotStart);
			var durationInHours = (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);
			CanvasPosition =  container.LengthOfOneHour * (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);
			
			var durationOfAppointment = Time.GetDurationBetween(appointment.StartTime, appointment.EndTime);
			ViewElementLength = container.LengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
		}

		public string PatientDisplayName
		{
			get { return appointment.Patient.Name; }
		}

		public Duration Duration { get; private set; }

		public double CanvasPosition
		{
			get { return canvasPosition; }
			set { PropertyChanged.ChangeAndNotify(this, ref canvasPosition, value); }
		}

		public double ViewElementLength
		{
			get { return viewElementLength; }
			set { PropertyChanged.ChangeAndNotify(this, ref viewElementLength, value); }
		}

		public Time StartTime { get; set; }
		public Time EndTime   { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

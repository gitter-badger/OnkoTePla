using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentViewModel : IAppointmentViewModel
	{
		private readonly ICommandBus commandBus;
		private readonly Appointment appointment;

		private ITherapyPlaceRowViewModel containerRow;
		private IAppointmentGridViewModel containerGrid;

		private double canvasPosition;
		private double viewElementLength;

		public AppointmentViewModel(ICommandBus commandBus, Appointment appointment, 
									ITherapyPlaceRowViewModel containerRow, IAppointmentGridViewModel containerGrid)
		{
			this.containerRow = containerRow;
			this.containerGrid = containerGrid;

			this.appointment = appointment;			
			this.commandBus = commandBus;

			containerRow.PropertyChanged += OnContainerChanged;

			OnContainerChanged(containerRow, null);
		}

		private void OnContainerChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var container = (ITherapyPlaceRowViewModel) sender;

			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointment.StartTime, container.TimeSlotStart);			
			CanvasPosition =  container.LengthOfOneHour * (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);
			
			var durationOfAppointment = Time.GetDurationBetween(appointment.StartTime, appointment.EndTime);
			ViewElementLength = container.LengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
		}

		public ICommand DeleteAppointment
		{
			get { throw new System.NotImplementedException(); }
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

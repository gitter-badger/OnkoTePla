using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
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
		private OperatingMode operatingMode;

		private readonly Command switchToEditModeCommand;

		public AppointmentViewModel(ICommandBus commandBus, Appointment appointment, 
									ITherapyPlaceRowViewModel containerRow, 
									IAppointmentGridViewModel containerGrid)
		{
			this.containerRow = containerRow;
			this.containerGrid = containerGrid;

			this.appointment = appointment;			
			this.commandBus = commandBus;

			containerRow.PropertyChanged += OnContainerChanged;

			OnContainerChanged(containerRow, null);

			switchToEditModeCommand = new Command(
				() =>
				{
					if (containerGrid.OperatingMode == OperatingMode.View)
					{
						containerGrid.EditingObject = this;
						OperatingMode = OperatingMode.Edit;
					}

				}
			);
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

		public ICommand SwitchToEditMode
		{
			get { return switchToEditModeCommand; }
		}

		public string PatientDisplayName
		{
			get { return appointment.Patient.Name; }
		}		

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

		public OperatingMode OperatingMode
		{
			get { return operatingMode; }
			private set { PropertyChanged.ChangeAndNotify(this, ref operatingMode, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

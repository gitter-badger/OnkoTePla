using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;
using MahApps.Metro.Controls.Dialogs;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentViewModel : IAppointmentViewModel
	{		
		private readonly Appointment appointment;

		private readonly ITherapyPlaceRowViewModel containerRow;
		private readonly IAppointmentGridViewModel containerGrid;

		private double canvasPosition;
		private double viewElementLength;
		private OperatingMode operatingMode;

		private readonly Command switchToEditModeCommand;
		private readonly Command deleteAppointmentCommand;

		public AppointmentViewModel(Appointment appointment, 
									ITherapyPlaceRowViewModel containerRow, 
									IAppointmentGridViewModel containerGrid)
		{
			this.containerRow = containerRow;
			this.containerGrid = containerGrid;

			this.appointment = appointment;						

			AttachContainerHander();

			OnContainerRowChanged(containerRow, null);

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

			deleteAppointmentCommand = new Command(async () =>
				{
					var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?", 
												   MessageBoxButton.OKCancel, MessageBoxImage.Question);
					var result = await dialog.ShowDialog();

				    if (result == MessageDialogResult.Affirmative)
				    {
				        containerGrid.DeleteAppointment(this, appointment, containerRow);
				    }
				}
			);
		}

		private void OnContainerGridChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "OperatingMode")			
				if (((IAppointmentGridViewModel)sender).OperatingMode == OperatingMode.View)
					if (OperatingMode == OperatingMode.Edit)
						OperatingMode = OperatingMode.View;			
		}

		private void OnContainerRowChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var container = (ITherapyPlaceRowViewModel) sender;

			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointment.StartTime, container.TimeSlotStart);			
			CanvasPosition =  container.LengthOfOneHour * (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);
			
			var durationOfAppointment = Time.GetDurationBetween(appointment.StartTime, appointment.EndTime);
			ViewElementLength = container.LengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
		}

		public ICommand DeleteAppointment { get { return deleteAppointmentCommand; }}
		public ICommand SwitchToEditMode  { get { return switchToEditModeCommand;  }}

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


		private void AttachContainerHander()
		{
			containerRow.PropertyChanged  += OnContainerRowChanged;
			containerGrid.PropertyChanged += OnContainerGridChanged;
		}

		private void DetachContainerHandler()
		{
			containerRow.PropertyChanged  -= OnContainerRowChanged;
			containerGrid.PropertyChanged -= OnContainerGridChanged;
		}

		// TODO: richtig so!?!?

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		 
		~AppointmentViewModel()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
			{
				if (disposing) 
					DetachContainerHandler();
								
			}
			disposed = true;						
		}
	}
}

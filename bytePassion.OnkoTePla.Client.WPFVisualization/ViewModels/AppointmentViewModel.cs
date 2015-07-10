using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

		private readonly IEnumerable<ITherapyPlaceRowViewModel> containerRows;
		private readonly IAppointmentGridViewModel containerGrid;

		private ITherapyPlaceRowViewModel currentRow;

		// hier wird die row eingetragen
		// die rows holen sich dann alle appointments und zeigen nur die an, wo die row passt
		//dfsdf

		private double canvasPosition;
		private double viewElementLength;
		private OperatingMode operatingMode;

		private readonly Command switchToEditModeCommand;
		private readonly Command deleteAppointmentCommand;

		public AppointmentViewModel(Appointment appointment,
									IEnumerable<ITherapyPlaceRowViewModel> containerRows, 						
									IAppointmentGridViewModel containerGrid)
		{
			this.containerRows = containerRows;
			this.containerGrid = containerGrid;
			this.appointment = appointment;
			CurrentRow = containerRows.First(rowModel => rowModel.TherapyPlaceId == appointment.TherapyPlace.Id);


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
					var result = await dialog.ShowMahAppsDialog();

				    if (result == MessageDialogResult.Affirmative)
				    {
						containerGrid.DeleteAppointment(this, appointment, CurrentRow);
				    }
				}
			);
		}

		private ITherapyPlaceRowViewModel CurrentRow
		{
			get { return currentRow; }
			set
			{
				if (currentRow != null)
				{
					currentRow.RemoveAppointment(this);
					DetachContainerHandler();
				}

				currentRow = value;
				
				currentRow.AddAppointment(this);
				AttachContainerHander();
				OnContainerRowChanged(currentRow, null);
			}
		}		

		private void OnContainerGridChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "OperatingMode")						//
				if (((IAppointmentGridViewModel)sender).OperatingMode == OperatingMode.View)	// Operating Mode to "OperatingMode.View"
					if (OperatingMode == OperatingMode.Edit)									// is set from the Grid
						OperatingMode = OperatingMode.View;										//
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

		public Guid AppointmentId  { get { return appointment.Id;              }}
		public Guid TherapyPlaceId { get { return appointment.TherapyPlace.Id; }}

		public OperatingMode OperatingMode
		{
			get { return operatingMode; }
			private set { PropertyChanged.ChangeAndNotify(this, ref operatingMode, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;


		private void AttachContainerHander()
		{
			CurrentRow.PropertyChanged    += OnContainerRowChanged;
			containerGrid.PropertyChanged += OnContainerGridChanged;
		}

		private void DetachContainerHandler()
		{
			CurrentRow.PropertyChanged    -= OnContainerRowChanged;
			containerGrid.PropertyChanged -= OnContainerGridChanged;
		}

		#region Dispose

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
				{					
					DetachContainerHandler();
					currentRow.RemoveAppointment(this);
				}

			}
			disposed = true;
		}

		#endregion
	}
}

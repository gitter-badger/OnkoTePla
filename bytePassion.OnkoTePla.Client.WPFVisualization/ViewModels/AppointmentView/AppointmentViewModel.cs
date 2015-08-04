using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Contracts.Appointments;
using MahApps.Metro.Controls.Dialogs;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public class AppointmentViewModel : IAppointmentViewModel
	{		
		private readonly Appointment appointment;

		private readonly IEnumerable<ITherapyPlaceRowViewModel> containerRows;
		private readonly IAppointmentGridViewModel containerGrid;

		private ITherapyPlaceRowViewModel currentRow;
		

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
			
			var globalGridSizeVariable  = Global.ViewModelCommunication.GetGlobalViewModelVariable<Size>(Global.AppointmentGridSizeVariable);
			globalGridSizeVariable.StateChanged  += OnGridSizeChanged;

			OnGridSizeChanged(globalGridSizeVariable.Value);			
		}		

		private void OnGridSizeChanged(Size newGridSize)
		{
			var lengthOfOneHour = newGridSize.Width / (Time.GetDurationBetween(CurrentRow.TimeSlotEnd, CurrentRow.TimeSlotStart).Seconds / 3600.0);
			
			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointment.StartTime, CurrentRow.TimeSlotStart);
			CanvasPosition =  lengthOfOneHour * (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);

			var durationOfAppointment = Time.GetDurationBetween(appointment.StartTime, appointment.EndTime);
			ViewElementLength = lengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
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
			}
		}		

		private void OnContainerGridChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "OperatingMode")						//
				if (((IAppointmentGridViewModel)sender).OperatingMode == OperatingMode.View)	// Operating Mode to "OperatingMode.View"
					if (OperatingMode == OperatingMode.Edit)									// is set from the Grid
						OperatingMode = OperatingMode.View;										//
		}		

		public ICommand DeleteAppointment { get { return deleteAppointmentCommand; }}
		public ICommand SwitchToEditMode  { get { return switchToEditModeCommand;  }}

		public string PatientDisplayName
		{
			get {return appointment.Patient.Name; }
		}

		public string TimeSpan
		{
			get { return appointment.StartTime.ToString().Substring(0, 5) + " - " + appointment.EndTime.ToString().Substring(0, 5); }
		}

		public string AppointmentDate => appointment.Day.ToString();
		public string Description     => appointment.Description;
		public string Room            => appointment.TherapyPlace.Name;		

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
			containerGrid.PropertyChanged += OnContainerGridChanged;
		}

		private void DetachContainerHandler()
		{			
			containerGrid.PropertyChanged -= OnContainerGridChanged;
		}

		#region Dispose

		private bool disposed = false;
	    private readonly StringBuilder stringBuilder = new StringBuilder();

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

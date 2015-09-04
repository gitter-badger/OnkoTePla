using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Messages;
using bytePassion.OnkoTePla.Contracts.Appointments;
using MahApps.Metro.Controls.Dialogs;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public class AppointmentViewModel : DisposingObject, 
										IAppointmentViewModel, 
										IViewModelMessageHandler<DisposeAppointmentViewModel>,
										IViewModelMessageHandler<NewSizeAvailable>
	{		
		private readonly Appointment appointment;		
		private readonly IDataCenter dataCenter;
		private readonly ViewModelCommunication<ViewModelMessage> viewModelCommunication;

		private double canvasLeftPosition;
		private double viewElementLength;

		private OperatingMode operatingMode;

		private Time timeSlotStart;
		private Time timeSlotEnd;

		private TherapyPlaceRowIdentifier currentLocation;

		public AppointmentViewModel(Appointment appointment,
									ViewModelCommunication<ViewModelMessage> viewModelCommunication,
									IDataCenter dataCenter,
									TherapyPlaceRowIdentifier initialLocalisation)
		{ 						
			this.appointment = appointment;
			this.viewModelCommunication = viewModelCommunication;
			this.dataCenter = dataCenter;


			viewModelCommunication.RegisterViewModelAtCollection<AppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				this	
			);

			SwitchToEditMode = new Command(
				() =>
				{


				}
			);

			DeleteAppointment = new Command(async () =>
				{
					var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?", 
												   MessageBoxButton.OKCancel, MessageBoxImage.Question);
					var result = await dialog.ShowMahAppsDialog();

				    if (result == MessageDialogResult.Affirmative)
				    {
					//	containerGrid.DeleteAppointment(this, appointment);
				    }
				}
			);
												
			SetNewLocation(initialLocalisation, true);		
		}		
		

		private void SetNewLocation(TherapyPlaceRowIdentifier therapyPlaceRowIdentifier, bool isInitialLocation)
		{
			if (!isInitialLocation)
			{
				viewModelCommunication.SendTo<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier, RemoveAppointmentFromTherapyPlaceRow>(
					TherapyPlaceRowViewModelCollection,
					currentLocation,
					new RemoveAppointmentFromTherapyPlaceRow(this)
				);
			}

			currentLocation = therapyPlaceRowIdentifier;

			var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(therapyPlaceRowIdentifier.PlaceAndDate.Date,
																		   therapyPlaceRowIdentifier.PlaceAndDate.MedicalPracticeId);

			timeSlotStart = medicalPractice.HoursOfOpening.GetOpeningTime(therapyPlaceRowIdentifier.PlaceAndDate.Date);
			timeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(therapyPlaceRowIdentifier.PlaceAndDate.Date);


			viewModelCommunication.SendTo<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier, AddAppointmentToTherapyPlaceRow>(
				TherapyPlaceRowViewModelCollection,
				therapyPlaceRowIdentifier,
				new AddAppointmentToTherapyPlaceRow(this)	
			);

			var globalGridSizeVariable  = viewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable
			);

			SetNewGridSize(globalGridSizeVariable.Value);
		}

		private void SetNewGridSize(Size newGridSize)
		{
			var lengthOfOneHour = newGridSize.Width / (Time.GetDurationBetween(timeSlotEnd, timeSlotStart).Seconds / 3600.0);
			
			var durationFromDayBeginToAppointmentStart = Time.GetDurationBetween(appointment.StartTime, timeSlotStart);
			CanvasLeftPosition =  lengthOfOneHour * (durationFromDayBeginToAppointmentStart.Seconds / 3600.0);

			var durationOfAppointment = Time.GetDurationBetween(appointment.StartTime, appointment.EndTime);
			ViewElementLength = lengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
		}

		public Guid Identifier => appointment.Id;
		
		public ICommand DeleteAppointment { get; }
		public ICommand SwitchToEditMode  { get; }

		public string PatientDisplayName => appointment.Patient.Name;
		public string TimeSpan           => $"{appointment.StartTime.ToString().Substring(0, 5)} - {appointment.EndTime.ToString().Substring(0, 5)}";
		public string AppointmentDate    => appointment.Day.ToString();
		public string Description        => appointment.Description;
		public string Room               => appointment.TherapyPlace.Name;		

		public double CanvasLeftPosition
		{
			get { return canvasLeftPosition; }
			set { PropertyChanged.ChangeAndNotify(this, ref canvasLeftPosition, value); }
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

		public void Process (DisposeAppointmentViewModel message)
		{
			Dispose();
		}

		public void Process (NewSizeAvailable message)
		{
			SetNewGridSize(message.NewSize);
		}

		public override void CleanUp()
		{
			viewModelCommunication.DeregisterViewModelAtCollection<AppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				this
			);

			viewModelCommunication.SendTo<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier, RemoveAppointmentFromTherapyPlaceRow>(
				TherapyPlaceRowViewModelCollection,
				currentLocation,
				new RemoveAppointmentFromTherapyPlaceRow(this)
			);
		}

		public event PropertyChangedEventHandler PropertyChanged;		
	}	
}

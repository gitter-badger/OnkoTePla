using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using MahApps.Metro.Controls.Dialogs;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using DeleteAppointment = bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages.DeleteAppointment;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public class AppointmentViewModel : DisposingObject, 
										IAppointmentViewModel										
	{		
		private readonly Appointment appointment;
		private readonly IDataCenter dataCenter;
		private readonly TherapyPlaceRowIdentifier initialLocalisation;

		private readonly IGlobalState<AppointmentModifications> currentModifiedAppointment;

		private TherapyPlaceRowIdentifier currentLocation;
		private OperatingMode operatingMode;

		private Time   timeSlotStart;
		private Time   timeSlotEnd;		
		private double gridWidth;
		private Time   beginTime;
		private Time   endTime;

		private AppointmentModifications currentAppointmentModification;
		private bool showDisabledOverlay;

		public AppointmentViewModel(Appointment appointment,
									IViewModelCommunication viewModelCommunication,
									IDataCenter dataCenter,									
									TherapyPlaceRowIdentifier initialLocalisation)
		{ 						
			this.appointment = appointment;
			this.dataCenter = dataCenter;
			this.initialLocalisation = initialLocalisation;
			ViewModelCommunication = viewModelCommunication;		

			currentModifiedAppointment = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				this	
			);

			SwitchToEditMode = new Command(() =>
				{
					if (currentModifiedAppointment.Value == null)
					{
						currentAppointmentModification = new AppointmentModifications(appointment,
																					  initialLocalisation.PlaceAndDate.MedicalPracticeId, 
																					  dataCenter, 
																					  viewModelCommunication);


						currentAppointmentModification.PropertyChanged += OnAppointmentModificationsPropertyChanged;
						currentModifiedAppointment.Value = currentAppointmentModification;
						OperatingMode = OperatingMode.Edit;
						currentModifiedAppointment.StateChanged += OnCurrentModifiedAppointmentChanged;
					}
				}
			);

			DeleteAppointment = new Command(async () =>
				{
					var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?", 
												   MessageBoxButton.OKCancel, MessageBoxImage.Question);
					var result = await dialog.ShowMahAppsDialog();

				    if (result == MessageDialogResult.Affirmative)
				    {
						currentModifiedAppointment.Value = null;

						viewModelCommunication.SendTo(
							AppointmentGridViewModelCollection,
							initialLocalisation.PlaceAndDate,
							new DeleteAppointment(appointment.Id, appointment.Patient.Id)
						);
					}					
				}
			);	

			BeginTime = appointment.StartTime;
			EndTime = appointment.EndTime;

			ShowDisabledOverlay = false;
			
			SetNewLocation(initialLocalisation, true);		
		}

		private void OnAppointmentModificationsPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var appointmentModifications = (AppointmentModifications)sender;
			switch (propertyChangedEventArgs.PropertyName)
			{
				case nameof(AppointmentModifications.BeginTime):
				{
					BeginTime = appointmentModifications.BeginTime;
					break;
				}
				case nameof(AppointmentModifications.EndTime):
				{
					EndTime = appointmentModifications.EndTime;
					break;
				}
				case nameof(AppointmentModifications.ShowDisabledOverlay):
				{
					ShowDisabledOverlay = appointmentModifications.ShowDisabledOverlay;
					break;
				}
				case nameof(AppointmentModifications.CurrentLocation):
				{
					SetNewLocation(appointmentModifications.CurrentLocation, false);
					break;
				}
			}
		}

		private void OnCurrentModifiedAppointmentChanged(AppointmentModifications newModifiedAppointment)
		{
			if (newModifiedAppointment == null || appointment != newModifiedAppointment.OriginalAppointment)
			{
				OperatingMode = OperatingMode.View;
				currentAppointmentModification.PropertyChanged -= OnAppointmentModificationsPropertyChanged;
				currentModifiedAppointment.StateChanged -= OnCurrentModifiedAppointmentChanged;
			}
		}


		private void SetNewLocation(TherapyPlaceRowIdentifier therapyPlaceRowIdentifier, bool isInitialLocation)
		{
			if (!isInitialLocation)
			{
				ViewModelCommunication.SendTo(
					TherapyPlaceRowViewModelCollection,
					currentLocation,
					new RemoveAppointmentFromTherapyPlaceRow(this)
				);
			}

			currentLocation = therapyPlaceRowIdentifier;

			var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(therapyPlaceRowIdentifier.PlaceAndDate.Date,
																		   therapyPlaceRowIdentifier.PlaceAndDate.MedicalPracticeId);

			TimeSlotBegin = medicalPractice.HoursOfOpening.GetOpeningTime(therapyPlaceRowIdentifier.PlaceAndDate.Date);
			TimeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(therapyPlaceRowIdentifier.PlaceAndDate.Date);


			ViewModelCommunication.SendTo(
				TherapyPlaceRowViewModelCollection,
				therapyPlaceRowIdentifier,
				new AddAppointmentToTherapyPlaceRow(this)	
			);

			var globalGridSizeVariable = ViewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable
			);

			GridWidth = globalGridSizeVariable.Value.Width;
		}


		public Guid Identifier => appointment.Id;
		
		public ICommand DeleteAppointment { get; }
		public ICommand SwitchToEditMode  { get; }


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

		public double GridWidth
		{
			get { return gridWidth; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gridWidth, value); }
		}

		public Time TimeSlotBegin
		{
			get { return timeSlotStart;}
			private set { PropertyChanged.ChangeAndNotify(this, ref timeSlotStart, value); }
			
		}

		public Time TimeSlotEnd
		{
			get { return timeSlotEnd; }
			private set { PropertyChanged.ChangeAndNotify(this, ref timeSlotEnd, value); }
		}


		public string PatientDisplayName => $"{appointment.Patient.Name} (*{appointment.Patient.Birthday.Year})";
        public string TimeSpan           => $"{appointment.StartTime.ToString().Substring(0, 5)} - {appointment.EndTime.ToString().Substring(0, 5)}";
		public string AppointmentDate    => appointment.Day.ToString();
		public string Description        => appointment.Description;
		public string Room               => appointment.TherapyPlace.Name;		
	

		public OperatingMode OperatingMode
		{
			get { return operatingMode; }
			private set { PropertyChanged.ChangeAndNotify(this, ref operatingMode, value); }
		}

		public bool ShowDisabledOverlay
		{
			get { return showDisabledOverlay; }
			private set { PropertyChanged.ChangeAndNotify(this, ref showDisabledOverlay, value); }
		}

		public void Process (Dispose message)
		{
			Dispose();
		}

		public void Process (NewSizeAvailable message)
		{
			GridWidth = message.NewSize.Width;
		}

		public void Process (RestoreOriginalValues message)
		{
			BeginTime = appointment.StartTime;
			EndTime   = appointment.EndTime;

			if (initialLocalisation != currentLocation)			
				SetNewLocation(initialLocalisation, false);

			ViewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			).Value = appointment.Day;
		}

		public override void CleanUp()
		{
			ViewModelCommunication.DeregisterViewModelAtCollection<AppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				this
			);

			ViewModelCommunication.SendTo(
				TherapyPlaceRowViewModelCollection,
				currentLocation,
				new RemoveAppointmentFromTherapyPlaceRow(this)
			);
		}

		public IViewModelCommunication ViewModelCommunication { get; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}	
}

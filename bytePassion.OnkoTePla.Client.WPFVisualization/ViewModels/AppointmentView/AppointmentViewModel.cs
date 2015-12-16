﻿using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.AppointmentModification;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using DeleteAppointment = bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages.DeleteAppointment;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
    public class AppointmentViewModel : ViewModel, 
										IAppointmentViewModel										
	{		
		private readonly Appointment appointment;
		private readonly TherapyPlaceRowIdentifier initialLocalisation;

		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
	    private readonly IGlobalState<Date> selectedDateVariable;

		private TherapyPlaceRowIdentifier currentLocation;
		private OperatingMode operatingMode;
		
		private Time   beginTime;
		private Time   endTime;

		
		private bool showDisabledOverlay;
		private AppointmentModifications currentAppointmentModifications;

		public AppointmentViewModel(Appointment appointment,
									IViewModelCommunication viewModelCommunication,																
									TherapyPlaceRowIdentifier initialLocalisation, 
                                    IGlobalState<AppointmentModifications> appointmentModificationsVariable,
                                    IGlobalState<Date> selectedDateVariable,									
                                    IAppointmentModificationsBuilder appointmentModificationsBuilder,
									AdornerControl adornerControl)
		{ 						
			this.appointment = appointment; 
			this.initialLocalisation = initialLocalisation;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		    this.selectedDateVariable = selectedDateVariable;
		    ViewModelCommunication = viewModelCommunication;
			AdornerControl = adornerControl;					

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				this	
			);

			SwitchToEditMode = new ParameterrizedCommand<bool>(isInitalAdjusting =>
				{
					if (appointmentModificationsVariable.Value == null)
					{
					    CurrentAppointmentModifications = appointmentModificationsBuilder.Build(appointment,
					                                                                            initialLocalisation.PlaceAndDate.MedicalPracticeId,
					                                                                            isInitalAdjusting); 

						CurrentAppointmentModifications.PropertyChanged += OnAppointmentModificationsPropertyChanged;
						appointmentModificationsVariable.Value = CurrentAppointmentModifications;
						OperatingMode = OperatingMode.Edit;
						appointmentModificationsVariable.StateChanged += OnCurrentModifiedAppointmentChanged;
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
						appointmentModificationsVariable.Value = null;

						viewModelCommunication.SendTo(
							AppointmentGridViewModelCollection,
							initialLocalisation.PlaceAndDate,
							new DeleteAppointment(appointment.Id, appointment.Patient.Id, ActionTag.RegularAction)
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
				CurrentAppointmentModifications.PropertyChanged -= OnAppointmentModificationsPropertyChanged;
				appointmentModificationsVariable.StateChanged -= OnCurrentModifiedAppointmentChanged;
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

			ViewModelCommunication.SendTo(
				TherapyPlaceRowViewModelCollection,
				therapyPlaceRowIdentifier,
				new AddAppointmentToTherapyPlaceRow(this)	
			);				
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

		
		public string PatientDisplayName => $"{appointment.Patient.Name} (*{appointment.Patient.Birthday.Year})";
        public string TimeSpan           => $"{appointment.StartTime.ToString().Substring(0, 5)} - {appointment.EndTime.ToString().Substring(0, 5)}";
		public string AppointmentDate    => appointment.Day.ToString();
		public string Description        => appointment.Description;
		public string Room               => appointment.TherapyPlace.Name;


		public AppointmentModifications CurrentAppointmentModifications												// TODO: evtl noch benachrichtigung von der Variable
		{
			get { return currentAppointmentModifications; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentAppointmentModifications, value); }
		}

		public AdornerControl AdornerControl { get; }


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

		#region process messages

		public void Process (Dispose message)
		{
			Dispose();
		}
		
		public void Process (RestoreOriginalValues message)
		{
			BeginTime = appointment.StartTime;
			EndTime   = appointment.EndTime;

			if (initialLocalisation != currentLocation)			
				SetNewLocation(initialLocalisation, false);
			
            selectedDateVariable.Value = appointment.Day;
		}

		public void Process (ShowDisabledOverlay message)
		{
			ShowDisabledOverlay = true;
		}

		public void Process (HideDisabledOverlay message)
		{
			ShowDisabledOverlay = false;
		}

		public void Process (SwitchToEditMode message)
		{
			SwitchToEditMode.Execute(false);
		}

		#endregion

		protected override void CleanUp()
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
        
		public override event PropertyChangedEventHandler PropertyChanged;		
	}	
}

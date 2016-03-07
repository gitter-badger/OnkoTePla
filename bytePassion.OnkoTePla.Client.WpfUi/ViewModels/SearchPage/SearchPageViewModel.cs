using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Enums;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Resources.UserNotificationService;
using MahApps.Metro.Controls.Dialogs;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage
{
	internal class SearchPageViewModel : ViewModel, 
                                         ISearchPageViewModel
    {
		private class AppointmentSorter : IComparer<AppointmentTransferData>
		{
			public int Compare (AppointmentTransferData a1, AppointmentTransferData a2)
			{
				return a1.Day == a2.Day
					? a1.StartTime.CompareTo(a2.StartTime)
					: a1.Day.CompareTo(a2.Day);
			}
		}

		private const string NoPatientSelected = "- noch kein Patient ausgewählt -";
		
		private readonly ISharedState<Date> selectedDateVariable;		
        private readonly ISharedStateReadOnly<Patient> selectedPatientVariable;        
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandService commandService;		
		private readonly IClientReadModelRepository readModelRepository;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;

		private readonly Action<string> errorCallBack;

		private AppointmentsOfAPatientReadModel currentReadModel;
		private string selectedPatient;
		private bool showPreviousAppointments;
		private bool noAppointmentsAvailable;

		public SearchPageViewModel(IPatientSelectorViewModel patientSelectorViewModel,
								   ISharedStateReadOnly<Patient> selectedPatientVariable,
								   ISharedState<Date> selectedDateVariable,								                                     
								   IViewModelCommunication viewModelCommunication,
								   ICommandService commandService,
								   IClientReadModelRepository readModelRepository,	
								   IClientMedicalPracticeRepository medicalPracticeRepository,							   
								   Action<string> errorCallBack)
		{
		    this.selectedPatientVariable = selectedPatientVariable;		    
			this.viewModelCommunication = viewModelCommunication;
			this.commandService = commandService;			
			this.readModelRepository = readModelRepository;
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.errorCallBack = errorCallBack;			
			this.selectedDateVariable = selectedDateVariable;

			NoAppointmentsAvailable = false;

			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			DeleteAppointment = new ParameterrizedCommand<AppointmentTransferData>(DoDeleteAppointment);
			ModifyAppointment = new ParameterrizedCommand<AppointmentTransferData>(DoModifyAppointment);
						
			SelectedPatient = NoPatientSelected;

			DisplayedAppointments = new ObservableCollection<AppointmentTransferData>();
		}

		private void DoModifyAppointment(AppointmentTransferData appointment)
		{						
			viewModelCommunication.Send(new AsureDayIsLoaded(appointment.MedicalPracticeId, 
															 appointment.Day, 
															 () =>
															 {
																 selectedDateVariable.Value = appointment.Day;

																 viewModelCommunication.SendTo(
																	Constants.ViewModelCollections.AppointmentViewModelCollection,
																	appointment.Id,
																	new SwitchToEditMode()	
																 );															  
															 }));
			
			viewModelCommunication.Send(new ShowPage(MainPage.Overview));
		}

		private async void DoDeleteAppointment(AppointmentTransferData appointment)
		{
			var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?",
										   MessageBoxButton.OKCancel);
			var result = await dialog.ShowMahAppsDialog();

			if (result == MessageDialogResult.Affirmative)
			{
				medicalPracticeRepository.RequestPraticeVersion(
					practiceVersion =>
					{
						commandService.TryDeleteAppointment(new AggregateIdentifier(appointment.Day,
																					appointment.MedicalPracticeId,
																					practiceVersion),
															appointment.PatientId,
															appointment.Id,
															appointment.Description,
															appointment.StartTime,
															appointment.EndTime,
															appointment.TherapyPlaceId,
															ActionTag.RegularAction,
															errorMsg =>
															{
																viewModelCommunication.Send(new ShowNotification($"Termin kann nicht gelöscht werden: {errorMsg}", 5));
															});
					},
					appointment.MedicalPracticeId,
					appointment.Day,
					errorCallBack
				);				
			}
		}

		private void OnSelectedPatientVariableChanged(Patient patient)
		{
			if (currentReadModel != null)
			{
				currentReadModel.AppointmentChanged -= OnCurrentReadModelAppointmentsChanged;
				DisplayedAppointments.Clear();
			}

			ShowPreviousAppointments = false;

			if (patient != null)
			{
				readModelRepository.RequestAppointmentsOfAPatientReadModel(
					patientReadModel =>
					{
						Application.Current.Dispatcher.Invoke(() =>
						{
							currentReadModel = patientReadModel;						
							currentReadModel.Appointments.Where(appointment => appointment.Day >= TimeTools.Today())
														 .Do(DisplayedAppointments.Add);

							CheckDisplayedAppointmentCount();

							currentReadModel.AppointmentChanged += OnCurrentReadModelAppointmentsChanged;

							DisplayedAppointments.Sort(new AppointmentSorter());
							SelectedPatient = patient.Name;
						});
					},
					patient.Id,
					errorCallBack						
				);				
			}
			else
			{
				SelectedPatient = NoPatientSelected;
			}
		}

		private void OnCurrentReadModelAppointmentsChanged(object sender, RawAppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			switch (appointmentChangedEventArgs.ChangeAction)
			{
				case ChangeAction.Added:
				{
					DisplayedAppointments.Add(appointmentChangedEventArgs.Appointment);
					break;
				}
				case ChangeAction.Deleted:
				{
					DisplayedAppointments.Remove(appointmentChangedEventArgs.Appointment);
					break;
				}
				case ChangeAction.Modified:
				{
					var oldAppointment = DisplayedAppointments.First(appointment => appointment.Id == appointmentChangedEventArgs.Appointment.Id);
					DisplayedAppointments.Remove(oldAppointment);
					DisplayedAppointments.Add(appointmentChangedEventArgs.Appointment);
					break;
				}
			}
			DisplayedAppointments.Sort(new AppointmentSorter());
			
		}

		private void CheckDisplayedAppointmentCount()
		{
			if (DisplayedAppointments != null)
			{
				NoAppointmentsAvailable = DisplayedAppointments.Count == 0;
			}
		}
		
		public ICommand DeleteAppointment { get; }
		public ICommand ModifyAppointment { get; }		

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }

		public bool ShowPreviousAppointments
		{
			get { return showPreviousAppointments; }
			set
			{
				if (showPreviousAppointments == value)
					return;

				if (value)
				{
					currentReadModel.Appointments.Where(appointment => !DisplayedAppointments.Contains(appointment))
												 .Do(appointment => DisplayedAppointments.Add(appointment));

					DisplayedAppointments.Sort(new AppointmentSorter());					
				}
				else
				{
					DisplayedAppointments.Where(appointment => appointment.Day < TimeTools.Today())
										 .ToList()
										 .Do(appointment => DisplayedAppointments.Remove(appointment));
				}

				CheckDisplayedAppointmentCount();

				PropertyChanged.ChangeAndNotify(this, ref showPreviousAppointments, value);
			}
		}

		public bool NoAppointmentsAvailable
		{
			get { return noAppointmentsAvailable; }
			private set { PropertyChanged.ChangeAndNotify(this, ref noAppointmentsAvailable, value); }
		}

		public string SelectedPatient
		{
			get { return selectedPatient; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
		}

		public ObservableCollection<AppointmentTransferData> DisplayedAppointments { get; }
		
	    protected override void CleanUp()
	    {
            selectedPatientVariable.StateChanged -= OnSelectedPatientVariableChanged;
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
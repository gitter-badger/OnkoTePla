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
		private readonly ISharedStateReadOnly<Guid> selectedMedicalPracticeIdVariable;
        private readonly ISharedStateReadOnly<Patient> selectedPatientVariable;        
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandService commandService;		
		private readonly IClientReadModelRepository readModelRepository;
		
		private readonly Action<string> errorCallBack;

		private AppointmentsOfAPatientReadModel currentReadModel;
		private string selectedPatient;

		public SearchPageViewModel(IPatientSelectorViewModel patientSelectorViewModel,
								   ISharedStateReadOnly<Patient> selectedPatientVariable,
								   ISharedState<Date> selectedDateVariable,
								   ISharedStateReadOnly<Guid> selectedMedicalPracticeIdVariable,                                   
								   IViewModelCommunication viewModelCommunication,
								   ICommandService commandService,
								   IClientReadModelRepository readModelRepository,								   
								   Action<string> errorCallBack)
		{
		    this.selectedPatientVariable = selectedPatientVariable;		    
			this.viewModelCommunication = viewModelCommunication;
			this.commandService = commandService;			
			this.readModelRepository = readModelRepository;
			
			this.errorCallBack = errorCallBack;
			this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
			this.selectedDateVariable = selectedDateVariable;

			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			DeleteAppointment = new ParameterrizedCommand<Appointment>(DoDeleteAppointment);
			ModifyAppointment = new ParameterrizedCommand<Appointment>(DoModifyAppointment);
						
			SelectedPatient = NoPatientSelected;

			DisplayedAppointments = new ObservableCollection<AppointmentTransferData>();
		}

		private void DoModifyAppointment(Appointment appointment)
		{			
			selectedDateVariable.Value = appointment.Day;

			viewModelCommunication.SendTo(
				Constants.AppointmentViewModelCollection,
				appointment.Id,
				new SwitchToEditMode()	
			);

			viewModelCommunication.Send(new ShowPage(MainPage.Overview));
		}

		private async void DoDeleteAppointment(Appointment appointment)
		{
			var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?",
										   MessageBoxButton.OKCancel);
			var result = await dialog.ShowMahAppsDialog();

			if (result == MessageDialogResult.Affirmative)
			{
				commandService.TryDeleteAppointment(new AggregateIdentifier(appointment.Day, 
																		    selectedMedicalPracticeIdVariable.Value),
																			appointment.Patient.Id,
													appointment.Id,
													appointment.Description,
													appointment.StartTime,
													appointment.EndTime,
													appointment.TherapyPlace.Id,
													ActionTag.RegularAction,
													errorMsg =>
													{
														viewModelCommunication.Send(new ShowNotification($"Termin kann nicht gelöscht werden: {errorMsg}", 5));
													});
			}
		}

		private void OnSelectedPatientVariableChanged(Patient patient)
		{
			if (currentReadModel != null)
			{
				currentReadModel.AppointmentChanged -= OnCurrentReadModelAppointmentsChanged;
				DisplayedAppointments.Clear();
			}

			if (patient != null)
			{
				readModelRepository.RequestAppointmentsOfAPatientReadModel(
					patientReadModel =>
					{
						Application.Current.Dispatcher.Invoke(() =>
						{
							currentReadModel = patientReadModel;						
							currentReadModel.Appointments.Do(DisplayedAppointments.Add);
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

		public ICommand DeleteAppointment { get; }
		public ICommand ModifyAppointment { get; }		

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }

		public bool ShowPreviousAppointments { get; set; }

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
﻿using System;
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
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Enums;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Patients;
using MahApps.Metro.Controls.Dialogs;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using DeleteAppointment = bytePassion.OnkoTePla.Client.Core.Domain.Commands.DeleteAppointment;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage
{
	public class SearchPageViewModel : ISearchPageViewModel
    {
		private class AppointmentSorter : IComparer<Appointment>
		{
			public int Compare (Appointment a1, Appointment a2)
			{
				return a1.Day == a2.Day
					? a1.StartTime.CompareTo(a2.StartTime)
					: a1.Day.CompareTo(a2.Day);
			}
		}

		private const string NoPatientSelected = "- noch kein Patient ausgewählt -";


		private readonly IGlobalState<Patient> selectedPatientVariable;
		private readonly ICommandBus commandBus;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IDataCenter dataCenter;

		private AppointmentsOfAPatientReadModel currentReadModel;
		private string selectedPatient;

		public SearchPageViewModel(IPatientSelectorViewModel patientSelectorViewModel,
								   IGlobalState<Patient> selectedPatientVariable,
								   ICommandBus commandBus,
								   IViewModelCommunication viewModelCommunication,
								   IDataCenter dataCenter)
		{
			this.selectedPatientVariable = selectedPatientVariable;
			this.commandBus = commandBus;
			this.viewModelCommunication = viewModelCommunication;
			this.dataCenter = dataCenter;
			selectedPatientVariable.StateChanged += OnSelectedPatientVariableChanged;

			PatientSelectorViewModel = patientSelectorViewModel;

			DeleteAppointment = new ParameterrizedCommand<Appointment>(DoDeleteAppointment);
			ModifyAppointment = new ParameterrizedCommand<Appointment>(DoModifyAppointment);
						
			SelectedPatient = NoPatientSelected;

			DisplayedAppointments = new ObservableCollection<Appointment>();
		}

		private void DoModifyAppointment(Appointment appointment)
		{
			var selectedDateVariable = viewModelCommunication.GetGlobalViewModelVariable<Date>(AppointmentGridSelectedDateVariable);
			selectedDateVariable.Value = appointment.Day;

			viewModelCommunication.SendTo(
				AppointmentViewModelCollection,
				appointment.Id,
				new SwitchToEditMode()	
			);

			viewModelCommunication.Send(new ShowPage(MainPage.Overview));
		}

		private async void DoDeleteAppointment(Appointment appointment)
		{
			var dialog = new UserDialogBox("", "Wollen Sie den Termin wirklich löschen?",
										   MessageBoxButton.OKCancel, MessageBoxImage.Question);
			var result = await dialog.ShowMahAppsDialog();

			if (result == MessageDialogResult.Affirmative)
			{
				var currentMedicalPracticeId = viewModelCommunication.GetGlobalViewModelVariable<Guid>(AppointmentGridDisplayedPracticeVariable).Value;
				var readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(new AggregateIdentifier(appointment.Day, currentMedicalPracticeId));

				commandBus.SendCommand(new DeleteAppointment(readModel.Identifier,
															 readModel.AggregateVersion,
															 dataCenter.SessionInfo.LoggedInUser.Id,
															 appointment.Patient.Id,
															 ActionTag.RegularAction,
															 appointment.Id));
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
				currentReadModel = dataCenter.ReadModelRepository.GetAppointmentsOfAPatientReadModel(patient.Id);
				currentReadModel.Appointments.Do(DisplayedAppointments.Add);
				currentReadModel.AppointmentChanged += OnCurrentReadModelAppointmentsChanged;

				DisplayedAppointments.Sort(new AppointmentSorter());
				SelectedPatient = patient.Name;
			}
			else
			{
				SelectedPatient = NoPatientSelected;
			}
		}

		private void OnCurrentReadModelAppointmentsChanged(object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
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

		public ObservableCollection<Appointment> DisplayedAppointments { get; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
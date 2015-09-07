﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Contracts.Appointments;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

using DeleteAppointmentCommand = bytePassion.OnkoTePla.Client.Core.Domain.Commands.DeleteAppointment;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public class AppointmentGridViewModel : DisposingObject,
											IAppointmentGridViewModel, 											
											IViewModelMessageHandler<Activate>,
											IViewModelMessageHandler<Deactivate>,
											IViewModelMessageHandler<DeleteAppointment>
	{
		private bool viewModelIsActive;

		private readonly IDataCenter dataCenter;
		private readonly ICommandBus commandBus;
		private readonly ViewModelCommunication<ViewModelMessage> viewModelCommunication;

		private readonly IDictionary<Guid, TherapyPlaceRowViewModel> availableTherapyPlaceRowViewModels; 

		private readonly AppointmentsOfADayReadModel readModel;
		private readonly IGlobalState<Size> globalGridSizeVariable;
		private readonly IGlobalState<Guid?> globalRoomFilterVariable; 

		public AppointmentGridViewModel(AggregateIdentifier identifier, 
									    IDataCenter dataCenter, 
										ICommandBus commandBus,
										ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{
			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			this.viewModelCommunication = viewModelCommunication;

			viewModelIsActive = false;
			
			globalGridSizeVariable = viewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable
				);
			globalGridSizeVariable.StateChanged += OnGridSizeChanged;

			globalRoomFilterVariable = viewModelCommunication.GetGlobalViewModelVariable<Guid?>(
				AppointmentGridRoomFilterVariable	
			);
			globalRoomFilterVariable.StateChanged += OnGlobalRoomFilterVariableChanged;

			viewModelCommunication.RegisterViewModelAtCollection<AppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this					
			);

			readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(identifier);

			Identifier = readModel.Identifier; // because now the identifier contains the correct Version

			readModel.AppointmentChanged += OnReadModelAppointmentChanged;


			TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication, 
													  dataCenter, globalGridSizeVariable.Value);

			var medicalPractice = dataCenter.Configuration.GetMedicalPracticeByIdAndVersion(Identifier.MedicalPracticeId,
			                                                                                Identifier.PracticeVersion);

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();
			availableTherapyPlaceRowViewModels = new Dictionary<Guid, TherapyPlaceRowViewModel>();

			foreach (var room in medicalPractice.Rooms)
			{
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					var location = new TherapyPlaceRowIdentifier(Identifier, therapyPlace.Id);
					availableTherapyPlaceRowViewModels.Add(therapyPlace.Id, new TherapyPlaceRowViewModel(viewModelCommunication, therapyPlace, 
																										 room.DisplayedColor, location));
				}
			}

			foreach (var appointment in readModel.Appointments)
			{
				AddAppointment(appointment);
			}

			OnGlobalRoomFilterVariableChanged(globalRoomFilterVariable.Value);
		}

		private void OnGlobalRoomFilterVariableChanged(Guid? newRoomFilter)
		{
			TherapyPlaceRowViewModels.Clear();

			if (newRoomFilter == null)
			{
				availableTherapyPlaceRowViewModels.Values
												  .Do(viewModel => TherapyPlaceRowViewModels.Add(viewModel));
			}
			else
			{
				var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(Identifier.Date, Identifier.MedicalPracticeId);

				medicalPractice.GetRoomById(newRoomFilter.Value)
							   .TherapyPlaces
							   .Select(therapyPlace => therapyPlace.Id)
							   .Do(id => TherapyPlaceRowViewModels.Add(availableTherapyPlaceRowViewModels[id]));								
			}
		}

		private void OnReadModelAppointmentChanged(object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			switch (appointmentChangedEventArgs.ChangeAction)
			{
				case ChangeAction.Added:   { AddAppointment(appointmentChangedEventArgs.Appointment);    break; }
				case ChangeAction.Deleted: { RemoveAppointment(appointmentChangedEventArgs.Appointment); break; }				
				case ChangeAction.Modified:
				{
					// TODO: das geht bestimmt besser!!
					RemoveAppointment(appointmentChangedEventArgs.Appointment);
					AddAppointment(appointmentChangedEventArgs.Appointment);
					break;
				}
			}
		}

		private void AddAppointment(Appointment newAppointment)
		{
			var location = new TherapyPlaceRowIdentifier(Identifier, newAppointment.TherapyPlace.Id);
			new AppointmentViewModel(newAppointment, viewModelCommunication, dataCenter, location);			
		}
		
		private void RemoveAppointment(Appointment appointmentToRemove)
		{
			viewModelCommunication.SendTo<AppointmentViewModel, Guid, Dispose>(
				AppointmentViewModelCollection,
				appointmentToRemove.Id,
				new Dispose()	
			);
		}

		private void OnGridSizeChanged(Size newGridSize)
		{
			if (viewModelIsActive)
			{
				viewModelCommunication.SendTo<TimeGridViewModel, AggregateIdentifier, NewSizeAvailable>(
					TimeGridViewModelCollection,
					Identifier,
					new NewSizeAvailable(newGridSize)	
				);

				foreach (var appointment in readModel.Appointments)
				{
					viewModelCommunication.SendTo<AppointmentViewModel, Guid, NewSizeAvailable>(
						AppointmentViewModelCollection,
						appointment.Id,
						new NewSizeAvailable(newGridSize)
					);
				}							
			}
		}

		public AggregateIdentifier Identifier { get; }
	
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; }

		public ITimeGridViewModel TimeGridViewModel { get; }
				
		
		public void Process(Activate message)
		{
			viewModelIsActive = true;
			OnGridSizeChanged(globalGridSizeVariable.Value);
		}

		public void Process(Deactivate message)
		{
			viewModelIsActive = false;
		}		

		public override void CleanUp()
		{			
			globalGridSizeVariable.StateChanged   -= OnGridSizeChanged;			
			globalRoomFilterVariable.StateChanged -= OnGlobalRoomFilterVariableChanged;			
			readModel.AppointmentChanged          -= OnReadModelAppointmentChanged;

			viewModelCommunication.DeregisterViewModelAtCollection<AppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this					
			);

			viewModelCommunication.SendTo<TimeGridViewModel, AggregateIdentifier, Dispose>(
				TimeGridViewModelCollection,
				Identifier,
				new Dispose()	
			);

			readModel.Appointments
					 .Do(RemoveAppointment);

			availableTherapyPlaceRowViewModels.Values
											  .Do(viewModel => viewModel.Dispose());			
		}

		public void Process(DeleteAppointment message)
		{
			commandBus.SendCommand(new DeleteAppointmentCommand(Identifier, 
																readModel.AggregateVersion, 
																dataCenter.SessionInfo.LoggedInUser.Id, 
																message.AppointmentId, 
																message.PatientId));
		}
	}
}
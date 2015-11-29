using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Contracts.Appointments;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using DeleteAppointment = bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages.DeleteAppointment;
using DeleteAppointmentCommand = bytePassion.OnkoTePla.Client.Core.Domain.Commands.DeleteAppointment;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public class AppointmentGridViewModel : DisposingObject,
											IAppointmentGridViewModel											
	{
		private bool isActive;

		private readonly IDataCenter dataCenter;
		private readonly ICommandBus commandBus;
		private readonly IViewModelCommunication viewModelCommunication;

		private readonly AppointmentsOfADayReadModel readModel;

		private readonly IGlobalState<Size> gridSizeVariable;
		private readonly IGlobalState<Guid?> roomFilterVariable;		
	    private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;		
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;

		public AppointmentGridViewModel(AggregateIdentifier identifier, 
									    IDataCenter dataCenter, 
										ICommandBus commandBus,
										IViewModelCommunication viewModelCommunication,
                                        IGlobalState<Size> gridSizeVariable,
										IGlobalState<Guid?> roomFilterVariable,
										IGlobalState<Date> selectedDateVariable,
                                        IGlobalState<AppointmentModifications> appointmentModificationsVariable,
										IGlobalState<Guid> selectedMedicalPracticeIdVariable,
										AdornerControl adornerControl,
										IAppointmentViewModelBuilder appointmentViewModelBuilder)
		{
			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			this.viewModelCommunication = viewModelCommunication;
		    this.gridSizeVariable = gridSizeVariable;
		    this.roomFilterVariable = roomFilterVariable;
            this.appointmentModificationsVariable = appointmentModificationsVariable;			
			this.appointmentViewModelBuilder = appointmentViewModelBuilder;			

	        IsActive = false;

			gridSizeVariable.StateChanged += OnGridSizeChanged;			
			roomFilterVariable.StateChanged += OnGlobalRoomFilterVariableChanged;

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this					
			);

			readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(identifier);

			Identifier = readModel.Identifier; // because now the identifier contains the correct Version

			readModel.AppointmentChanged += OnReadModelAppointmentChanged;

			TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication, 
													  dataCenter, gridSizeVariable.Value);

			var medicalPractice = dataCenter.Configuration.GetMedicalPracticeByIdAndVersion(Identifier.MedicalPracticeId,
			                                                                                Identifier.PracticeVersion);

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();			

			foreach (var room in medicalPractice.Rooms)
			{
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					var location = new TherapyPlaceRowIdentifier(Identifier, therapyPlace.Id);
					TherapyPlaceRowViewModels.Add(new TherapyPlaceRowViewModel(viewModelCommunication,
																			   dataCenter, 
																			   therapyPlace, 																										 
																			   room.DisplayedColor, 
																			   location,
																			   adornerControl,
																			   appointmentModificationsVariable,
																			   selectedDateVariable,
																			   selectedMedicalPracticeIdVariable));
				}
			}

			foreach (var appointment in readModel.Appointments)
			{
				AddAppointment(appointment);
			}			

			OnGlobalRoomFilterVariableChanged(roomFilterVariable.Value);

			PracticeIsClosedAtThisDay = false;
		}

		private void OnGlobalRoomFilterVariableChanged(Guid? newRoomFilter)
		{			
			if (newRoomFilter == null)
			{
				TherapyPlaceRowViewModels.Do(viewModel =>
				{
					viewModelCommunication.SendTo(
						TherapyPlaceRowViewModelCollection,
						viewModel.Identifier,
						new SetVisibility(true) 	
					);                             
				});
			}
			else
			{
				TherapyPlaceRowViewModels.Do(viewModel =>
				{
					viewModelCommunication.SendTo(
						TherapyPlaceRowViewModelCollection,
						viewModel.Identifier,
						new SetVisibility(false)
					);
				});

				var medicalPractice = dataCenter.GetMedicalPracticeByDateAndId(Identifier.Date, Identifier.MedicalPracticeId);

				medicalPractice.GetRoomById(newRoomFilter.Value)
							   .TherapyPlaces
							   .Select(therapyPlace => therapyPlace.Id)
							   .Do(id =>
							       {
									   viewModelCommunication.SendTo(
											TherapyPlaceRowViewModelCollection,
											new TherapyPlaceRowIdentifier(Identifier, id), 
											new SetVisibility(true)
									   );
								   });								
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
					RemoveAppointment(appointmentChangedEventArgs.Appointment);
					AddAppointment(appointmentChangedEventArgs.Appointment);
					break;
				}
			}
		}

		private void AddAppointment(Appointment newAppointment)
		{
			appointmentViewModelBuilder.Build(newAppointment, Identifier);			
		}
		
		private void RemoveAppointment(Appointment appointmentToRemove)
		{
			viewModelCommunication.SendTo( 
				AppointmentViewModelCollection, 
				appointmentToRemove.Id, 
				new Dispose()
			);
		}

		private void OnGridSizeChanged(Size newGridSize)
		{
			if (IsActive)
			{
				viewModelCommunication.SendTo(
					TimeGridViewModelCollection,
					Identifier,
					new NewSizeAvailable(newGridSize)	
				);

				foreach (var therapyPlaceRowIdentifier in TherapyPlaceRowViewModels.Select(viewModel => viewModel.Identifier))
				{ 
					viewModelCommunication.SendTo(
						TherapyPlaceRowViewModelCollection,
						therapyPlaceRowIdentifier,
						new NewSizeAvailable(newGridSize)
					);
				}							
			}
		}

		public AggregateIdentifier Identifier { get; }
	
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; }

		public ITimeGridViewModel TimeGridViewModel { get; }

		public bool PracticeIsClosedAtThisDay { get; }

		public bool IsActive
		{
			get { return isActive; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref isActive, value);
			}
		}


		public void Process(Activate message)
		{
			IsActive = true;
			OnGridSizeChanged(gridSizeVariable.Value);
		}

		public void Process(Deactivate message)
		{
			IsActive = false;
		}

        protected override void CleanUp()
		{			
			gridSizeVariable.StateChanged   -= OnGridSizeChanged;			
			roomFilterVariable.StateChanged -= OnGlobalRoomFilterVariableChanged;			
			readModel.AppointmentChanged    -= OnReadModelAppointmentChanged;

			viewModelCommunication.DeregisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				this					
			);

			viewModelCommunication.SendTo(
				TimeGridViewModelCollection,
				Identifier,
				new Dispose()	
			);

			readModel.Appointments
					 .Do(RemoveAppointment);

			readModel.Dispose();

			TherapyPlaceRowViewModels.Do(viewModel => viewModel.Dispose());			
		}

		public void Process(DeleteAppointment message)
		{
			commandBus.SendCommand(new DeleteAppointmentCommand(Identifier, 
																readModel.AggregateVersion, 
																dataCenter.SessionInfo.LoggedInUser.Id, 
																message.PatientId,
																message.ActionTag,
                                                                message.AppointmentId));
		}
		
		public void Process(SendCurrentChangesToCommandBus message)
		{			
			if (appointmentModificationsVariable.Value == null)
				return;

			var originalAppointment = appointmentModificationsVariable.Value.OriginalAppointment;

			if (originalAppointment.Description     == appointmentModificationsVariable.Value.Description &&
				originalAppointment.StartTime       == appointmentModificationsVariable.Value.BeginTime &&
				originalAppointment.EndTime         == appointmentModificationsVariable.Value.EndTime &&
				originalAppointment.Day             == appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate.Date &&
				originalAppointment.TherapyPlace.Id == appointmentModificationsVariable.Value.CurrentLocation.TherapyPlaceId)
			{
				return; // no changes to report
			}

			AggregateIdentifier sourceAggregateId;
			AggregateIdentifier destinationAggregateId = Identifier;

			uint sourceAggregateVersion;
			uint destinationAggregateVersion = readModel.AggregateVersion;


			if (appointmentModificationsVariable.Value.OriginalAppointment.Day != Identifier.Date)
			{
				sourceAggregateId = new AggregateIdentifier(appointmentModificationsVariable.Value.OriginalAppointment.Day, 
															Identifier.MedicalPracticeId);

				var tmpReadModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(sourceAggregateId);

				sourceAggregateVersion = tmpReadModel.AggregateVersion;
			}
			else
			{
				sourceAggregateId = destinationAggregateId;
				sourceAggregateVersion = destinationAggregateVersion;
			}

			commandBus.SendCommand(new ReplaceAppointment(sourceAggregateId, destinationAggregateId,
														  sourceAggregateVersion, destinationAggregateVersion,
														  dataCenter.SessionInfo.LoggedInUser.Id,
														  originalAppointment.Patient.Id, 
														  ActionTag.RegularAction, 
														  appointmentModificationsVariable.Value.Description,
														  appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate.Date,
														  appointmentModificationsVariable.Value.BeginTime,
														  appointmentModificationsVariable.Value.EndTime,															 
														  appointmentModificationsVariable.Value.CurrentLocation.TherapyPlaceId,
														  originalAppointment.Id,
														  originalAppointment.Day));
		}

		public void Process (CreateNewAppointmentFromModificationsAndSendToCommandBus message)
		{			
			commandBus.SendCommand(new AddAppointment(appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate, 
													  readModel.AggregateVersion, 
													  dataCenter.SessionInfo.LoggedInUser.Id, 
													  ActionTag.RegularAction, 
													  appointmentModificationsVariable.Value.OriginalAppointment.Patient.Id, 
													  appointmentModificationsVariable.Value.Description, 
													  appointmentModificationsVariable.Value.BeginTime, 
													  appointmentModificationsVariable.Value.EndTime, 
													  appointmentModificationsVariable.Value.CurrentLocation.TherapyPlaceId));			
		}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
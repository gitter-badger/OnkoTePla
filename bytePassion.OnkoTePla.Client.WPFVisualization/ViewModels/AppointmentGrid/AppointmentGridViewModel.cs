using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
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

		private readonly IDictionary<Guid, TherapyPlaceRowViewModel> availableTherapyPlaceRowViewModels; 

		private readonly AppointmentsOfADayReadModel readModel;

		private readonly IGlobalState<Size> globalGridSizeVariable;
		private readonly IGlobalState<Guid?> globalRoomFilterVariable;

		private readonly IReadOnlyList<TherapyPlaceRowIdentifier> therapyPlaceRowIdentifiers;

		public AppointmentGridViewModel(AggregateIdentifier identifier, 
									    IDataCenter dataCenter, 
										ICommandBus commandBus,
										IViewModelCommunication viewModelCommunication)
		{
			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			this.viewModelCommunication = viewModelCommunication;

			IsActive = false;
			
			globalGridSizeVariable = viewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable
			);
			globalGridSizeVariable.StateChanged += OnGridSizeChanged;

			globalRoomFilterVariable = viewModelCommunication.GetGlobalViewModelVariable<Guid?>(
				AppointmentGridRoomFilterVariable	
			);
			globalRoomFilterVariable.StateChanged += OnGlobalRoomFilterVariableChanged;

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
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
					availableTherapyPlaceRowViewModels.Add(therapyPlace.Id, new TherapyPlaceRowViewModel(viewModelCommunication,dataCenter, therapyPlace, 
																										 room.DisplayedColor, location));
				}
			}

			foreach (var appointment in readModel.Appointments)
			{
				AddAppointment(appointment);
			}

			therapyPlaceRowIdentifiers = medicalPractice.Rooms
			                                            .SelectMany(room => room.TherapyPlaces)
			                                            .Select(therapyPlaceRow => new TherapyPlaceRowIdentifier(Identifier, therapyPlaceRow.Id))
			                                            .ToList();

			OnGlobalRoomFilterVariableChanged(globalRoomFilterVariable.Value);

			PracticeIsClosedAtThisDay = false;
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

				foreach (var therapyPlaceRowIdentifier in therapyPlaceRowIdentifiers)
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
			OnGridSizeChanged(globalGridSizeVariable.Value);
		}

		public void Process(Deactivate message)
		{
			IsActive = false;
		}		

		public override void CleanUp()
		{			
			globalGridSizeVariable.StateChanged   -= OnGridSizeChanged;			
			globalRoomFilterVariable.StateChanged -= OnGlobalRoomFilterVariableChanged;			
			readModel.AppointmentChanged          -= OnReadModelAppointmentChanged;

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
		
		public void Process(SendCurrentChangesToCommandBus message)
		{
			var appointmentModificationVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable	
			);

			if (appointmentModificationVariable.Value == null)
				return;

			var originalAppointment = appointmentModificationVariable.Value.OriginalAppointment;

			if (originalAppointment.Description     == appointmentModificationVariable.Value.Description &&
				originalAppointment.StartTime       == appointmentModificationVariable.Value.BeginTime &&
				originalAppointment.EndTime         == appointmentModificationVariable.Value.EndTime &&
				originalAppointment.Day             == appointmentModificationVariable.Value.CurrentLocation.PlaceAndDate.Date &&
				originalAppointment.TherapyPlace.Id == appointmentModificationVariable.Value.CurrentLocation.TherapyPlaceId)
			{
				return; // no changes to report
			}

			AggregateIdentifier sourceAggregateId;
			AggregateIdentifier destinationAggregateId = Identifier;

			uint sourceAggregateVersion;
			uint destinationAggregateVersion = readModel.AggregateVersion;


			if (appointmentModificationVariable.Value.OriginalAppointment.Day != Identifier.Date)
			{
				sourceAggregateId = new AggregateIdentifier(appointmentModificationVariable.Value.OriginalAppointment.Day, 
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
														  appointmentModificationVariable.Value.Description,
														  appointmentModificationVariable.Value.CurrentLocation.PlaceAndDate.Date,
														  appointmentModificationVariable.Value.BeginTime,
														  appointmentModificationVariable.Value.EndTime,															 
														  appointmentModificationVariable.Value.CurrentLocation.TherapyPlaceId,
														  originalAppointment.Id,
														  originalAppointment.Day));
		}

		public void Process (CreateNewAppointmentFromModificationsAndSendToCommandBus message)
		{
			var appointmentModificationVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);

			if (appointmentModificationVariable.Value == null)
				throw new Exception("internal error");
			
			commandBus.SendCommand(new AddAppointment(appointmentModificationVariable.Value.CurrentLocation.PlaceAndDate, 
													  readModel.AggregateVersion, 
													  dataCenter.SessionInfo.LoggedInUser.Id, 
													  appointmentModificationVariable.Value.OriginalAppointment.Patient.Id, 
													  appointmentModificationVariable.Value.Description, 
													  appointmentModificationVariable.Value.BeginTime, 
													  appointmentModificationVariable.Value.EndTime, 
													  appointmentModificationVariable.Value.CurrentLocation.TherapyPlaceId));			
		}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
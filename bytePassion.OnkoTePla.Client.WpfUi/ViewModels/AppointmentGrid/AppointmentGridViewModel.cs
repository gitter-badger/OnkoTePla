using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Domain.Commands;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Readmodels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DeleteAppointment = bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages.DeleteAppointment;
using DeleteAppointmentCommand = bytePassion.OnkoTePla.Core.Domain.Commands.DeleteAppointment;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid
{
    internal class AppointmentGridViewModel : ViewModel,
											  IAppointmentGridViewModel											
	{
		private bool isActive;

		private readonly IDataCenter dataCenter;
		private readonly ICommandBus commandBus;
		private readonly IViewModelCommunication viewModelCommunication;		
		private readonly IGlobalStateReadOnly<Size> gridSizeVariable;
		private readonly IGlobalStateReadOnly<Guid?> roomFilterVariable;		
	    private readonly IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable;		
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;

        private readonly AppointmentsOfADayReadModel readModel;

        public AppointmentGridViewModel(AggregateIdentifier identifier, 
									    IDataCenter dataCenter, 
										ICommandBus commandBus,
										IViewModelCommunication viewModelCommunication,
                                        IGlobalStateReadOnly<Size> gridSizeVariable,
										IGlobalStateReadOnly<Guid?> roomFilterVariable,										
                                        IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable,										
										IAppointmentViewModelBuilder appointmentViewModelBuilder,
										ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder)
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
				Constants.AppointmentGridViewModelCollection,
				this					
			);

			readModel = dataCenter.GetAppointmentsOfADayReadModel(identifier);

			Identifier = readModel.Identifier; // because now the identifier contains the correct Version

			readModel.AppointmentChanged += OnReadModelAppointmentChanged;

			TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication, 
													  dataCenter, gridSizeVariable.Value);

			var medicalPractice = dataCenter.GetMedicalPracticeByIdAndVersion(Identifier.MedicalPracticeId,
			                                                                  Identifier.PracticeVersion);

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();			

			foreach (var room in medicalPractice.Rooms)
			{
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					var location = new TherapyPlaceRowIdentifier(Identifier, therapyPlace.Id);

					TherapyPlaceRowViewModels.Add(therapyPlaceRowViewModelBuilder.Build(therapyPlace, room, location));					
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
						Constants.TherapyPlaceRowViewModelCollection,
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
						Constants.TherapyPlaceRowViewModelCollection,
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
											Constants.TherapyPlaceRowViewModelCollection,
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
				Constants.AppointmentViewModelCollection, 
				appointmentToRemove.Id, 
				new Dispose()
			);
		}

		private void OnGridSizeChanged(Size newGridSize)
		{
			if (IsActive)
			{
				viewModelCommunication.SendTo(
					Constants.TimeGridViewModelCollection,
					Identifier,
					new NewSizeAvailable(newGridSize)	
				);

				foreach (var therapyPlaceRowIdentifier in TherapyPlaceRowViewModels.Select(viewModel => viewModel.Identifier))
				{ 
					viewModelCommunication.SendTo(
						Constants.TherapyPlaceRowViewModelCollection,
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

		public void Process(DeleteAppointment message)
		{
			commandBus.SendCommand(new DeleteAppointmentCommand(Identifier, 
																readModel.AggregateVersion, 
																dataCenter.LoggedInUser.Id, 
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

				var tmpReadModel = dataCenter.GetAppointmentsOfADayReadModel(sourceAggregateId);

				sourceAggregateVersion = tmpReadModel.AggregateVersion;
			}
			else
			{
				sourceAggregateId = destinationAggregateId;
				sourceAggregateVersion = destinationAggregateVersion;
			}

			commandBus.SendCommand(new ReplaceAppointment(sourceAggregateId, destinationAggregateId,
														  sourceAggregateVersion, destinationAggregateVersion,
														  dataCenter.LoggedInUser.Id,
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
													  dataCenter.LoggedInUser.Id, 
													  ActionTag.RegularAction, 
													  appointmentModificationsVariable.Value.OriginalAppointment.Patient.Id, 
													  appointmentModificationsVariable.Value.Description, 
													  appointmentModificationsVariable.Value.BeginTime, 
													  appointmentModificationsVariable.Value.EndTime, 
													  appointmentModificationsVariable.Value.CurrentLocation.TherapyPlaceId));			
		}

        protected override void CleanUp()
        {
            gridSizeVariable.StateChanged -= OnGridSizeChanged;
            roomFilterVariable.StateChanged -= OnGlobalRoomFilterVariableChanged;
            readModel.AppointmentChanged -= OnReadModelAppointmentChanged;

            viewModelCommunication.DeregisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
                Constants.AppointmentGridViewModelCollection,
                this
            );

            viewModelCommunication.SendTo(
                Constants.TimeGridViewModelCollection,
                Identifier,
                new Dispose()
            );

            readModel.Appointments
                     .Do(RemoveAppointment);

            readModel.Dispose();

            TherapyPlaceRowViewModels.Do(viewModel => viewModel.Dispose());
        }

        public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
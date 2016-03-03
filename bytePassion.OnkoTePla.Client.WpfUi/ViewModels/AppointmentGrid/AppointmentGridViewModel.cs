using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Notification;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.ReadModelRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using AppointmentsOfADayReadModel = bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.AppointmentsOfADayReadModel;

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid
{
	internal class AppointmentGridViewModel : ViewModel,
											  IAppointmentGridViewModel											
	{
		private bool isActive;

		private readonly ClientMedicalPracticeData medicalPractice;		
		private readonly IViewModelCommunication viewModelCommunication;		
		private readonly ISharedStateReadOnly<Size> gridSizeVariable;
		private readonly ISharedStateReadOnly<Guid?> roomFilterVariable;			    
		private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		private readonly Action<string> errorCallback;
		  
		private AppointmentsOfADayReadModel readModel;
		private ITimeGridViewModel timeGridViewModel;
		private ObservableCollection<ITherapyPlaceRowViewModel> therapyPlaceRowViewModels;
		 
		public AppointmentGridViewModel(AggregateIdentifier identifier, 
									    ClientMedicalPracticeData medicalPractice, 										
										IClientReadModelRepository readModelRepository,									
										IViewModelCommunication viewModelCommunication,
                                        ISharedStateReadOnly<Size> gridSizeVariable,
										ISharedStateReadOnly<Guid?> roomFilterVariable,
										IAppointmentViewModelBuilder appointmentViewModelBuilder,
										ITherapyPlaceRowViewModelBuilder therapyPlaceRowViewModelBuilder,
										Action<string> errorCallback)
		{
			this.medicalPractice = medicalPractice;			
			this.viewModelCommunication = viewModelCommunication;
		    this.gridSizeVariable = gridSizeVariable;
		    this.roomFilterVariable = roomFilterVariable;            		
			this.appointmentViewModelBuilder = appointmentViewModelBuilder;
	        this.errorCallback = errorCallback;

	        IsActive = false;
			PracticeIsClosedAtThisDay = false;

			gridSizeVariable.StateChanged += OnGridSizeChanged;			
			roomFilterVariable.StateChanged += OnGlobalRoomFilterVariableChanged;

			viewModelCommunication.RegisterViewModelAtCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				Constants.AppointmentGridViewModelCollection,
				this					
			);

			Identifier = identifier;

			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>();

			readModelRepository.RequestAppointmentsOfADayReadModel(
				newReadModel =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						readModel = newReadModel;

						readModel.AppointmentChanged += OnReadModelAppointmentChanged;

						TimeGridViewModel = new TimeGridViewModel(Identifier, viewModelCommunication,
																  medicalPractice, gridSizeVariable.Value);
						

						var requestedViewModels = 0;
						var buildedViewModels = 0;

						foreach (var room in medicalPractice.Rooms)
						{
							foreach (var therapyPlace in room.TherapyPlaces)
							{
								var location = new TherapyPlaceRowIdentifier(Identifier, therapyPlace.Id);
								requestedViewModels++;

								therapyPlaceRowViewModelBuilder.RequestBuild(
									viewModel =>
									{
										TherapyPlaceRowViewModels.Add(viewModel);
										buildedViewModels++;

										viewModelCommunication.SendTo(
											Constants.TherapyPlaceRowViewModelCollection,
											location,
											new NewSizeAvailable(gridSizeVariable.Value)
										);
									},
									therapyPlace,
									room,
									location,
									errorCallback
								);
							}
						}

						if (requestedViewModels != buildedViewModels)
							throw new NotImplementedException("kann wohl doch passieren ....");

						foreach (var appointment in readModel.Appointments)
						{
							AddAppointment(appointment);
						}

						OnGlobalRoomFilterVariableChanged(roomFilterVariable.Value);
					});
				},
				identifier,
				errorCallback	 				 
			);						
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
			appointmentViewModelBuilder.Build(newAppointment, Identifier, errorCallback);			
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

		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels
		{
			get { return therapyPlaceRowViewModels; }
			private set { PropertyChanged.ChangeAndNotify(this, ref therapyPlaceRowViewModels, value); }
		}

		public ITimeGridViewModel TimeGridViewModel
		{
			get { return timeGridViewModel; }
			private set { PropertyChanged.ChangeAndNotify(this, ref timeGridViewModel, value); }
		}

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
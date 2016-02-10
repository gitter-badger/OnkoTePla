using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.LocalSettings;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessageHandler;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel
{
	internal class MainViewModelBuilder : IMainViewModelBuilder
    {
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly IClientPatientRepository patientRepository;
		private readonly ICommandBus commandBus;
		private readonly ILocalSettingsRepository localSettingsRepository;
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISession session;		
        private readonly AdornerControl adornerControl;

		private ConfirmChangesMessageHandler confirmChangesMessageHandler;
		private RejectChangesMessageHandler rejectChangesMessageHandler;
		
		public MainViewModelBuilder(IClientMedicalPracticeRepository medicalPracticeRepository,
									IClientReadModelRepository readModelRepository,
									IClientPatientRepository patientRepository,
									ICommandBus commandBus,
									ILocalSettingsRepository localSettingsRepository,
                                    IViewModelCommunication viewModelCommunication,
									ISession session,                                   
                                    AdornerControl adornerControl)
        {
			this.medicalPracticeRepository = medicalPracticeRepository;
			this.readModelRepository = readModelRepository;
			this.patientRepository = patientRepository;
			this.commandBus = commandBus;
			this.localSettingsRepository = localSettingsRepository;
			this.viewModelCommunication = viewModelCommunication;
	        this.session = session;	        
            this.adornerControl = adornerControl;
        }



        public IMainViewModel Build(Action<string> errorCallback) 
        {            
            // Register Global ViewModelVariables

	        var firstDispayedDate = TimeTools.Today();	// TODO find last open

	        var lastUsedMedicalPracticeId = localSettingsRepository.LastUsedMedicalPracticeId;

	        if (lastUsedMedicalPracticeId == Guid.Empty)
	        {
		        lastUsedMedicalPracticeId = session.LoggedInUser.ListOfAccessablePractices.First();
	        }
	       			    
            var gridSizeVariable                  = new SharedState<Size>(new Size(400, 400));
            var selectedDateVariable              = new SharedState<Date>(firstDispayedDate);    
            var selectedMedicalPracticeIdVariable = new SharedState<Guid>(lastUsedMedicalPracticeId);
            var roomFilterVariable                = new SharedState<Guid?>();
            var appointmentModificationsVariable  = new SharedState<AppointmentModifications>();

            var selectedPatientForAppointmentSearchVariable = new SharedState<Patient>();


            // Create ViewModelCollection

            viewModelCommunication.CreateViewModelCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
                Constants.TherapyPlaceRowViewModelCollection
            );

            viewModelCommunication.CreateViewModelCollection<IAppointmentGridViewModel, AggregateIdentifier>(
                Constants.AppointmentGridViewModelCollection
            );

            viewModelCommunication.CreateViewModelCollection<ITimeGridViewModel, AggregateIdentifier>(
                Constants.TimeGridViewModelCollection
            );

            viewModelCommunication.CreateViewModelCollection<IAppointmentViewModel, Guid>(
                Constants.AppointmentViewModelCollection
            );            

            // build factorys

            var appointmentModificationsBuilder = new AppointmentModificationsBuilder(medicalPracticeRepository, 
																					  readModelRepository,
                                                                                      viewModelCommunication,
                                                                                      selectedDateVariable,
                                                                                      gridSizeVariable);

            var appointmentViewModelBuilder = new AppointmentViewModelBuilder(viewModelCommunication,
                                                                              appointmentModificationsVariable,
                                                                              selectedDateVariable,
                                                                              adornerControl,
                                                                              appointmentModificationsBuilder);

            var therapyPlaceRowViewModelBuilder = new TherapyPlaceRowViewModelBuilder(viewModelCommunication,
                                                                                      medicalPracticeRepository,
                                                                                      adornerControl,
                                                                                      appointmentModificationsVariable);

            var appointmentGridViewModelBuilder = new AppointmentGridViewModelBuilder(session,
																					  medicalPracticeRepository,
																					  readModelRepository,
																					  commandBus,
                                                                                      viewModelCommunication,                                                                                      
                                                                                      gridSizeVariable,
                                                                                      roomFilterVariable,
                                                                                      appointmentModificationsVariable,
                                                                                      appointmentViewModelBuilder,
                                                                                      therapyPlaceRowViewModelBuilder);
			
            var addAppointmentDialogWindowBuilder = new AddAppointmentDialogWindowBuilder(patientRepository,
																						  readModelRepository,
																						  medicalPracticeRepository,
                                                                                          selectedMedicalPracticeIdVariable,
                                                                                          selectedDateVariable,
                                                                                          appointmentViewModelBuilder);

            // build stand-alone viewModelMessageHandler

	        confirmChangesMessageHandler = new ConfirmChangesMessageHandler(viewModelCommunication,
																		    appointmentModificationsVariable,
																		    selectedMedicalPracticeIdVariable);

	        rejectChangesMessageHandler = new RejectChangesMessageHandler(viewModelCommunication,
																		  appointmentModificationsVariable);
			

            // build factories



            // create permanent ViewModels

            var dateDisplayViewModel = new DateDisplayViewModel(selectedDateVariable);

            var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(session, 
																						medicalPracticeRepository,
                                                                                        selectedMedicalPracticeIdVariable,
                                                                                        appointmentModificationsVariable, 
																						errorCallback);
			
            var roomSelectorViewModel = new RoomFilterViewModel(medicalPracticeRepository,
                                                                roomFilterVariable,
                                                                selectedDateVariable,
                                                                selectedMedicalPracticeIdVariable,
																errorCallback);

            var dateSelectorViewModel = new DateSelectorViewModel(selectedDateVariable);

            var gridContainerViewModel = new GridContainerViewModel(viewModelCommunication,
																	medicalPracticeRepository,
                                                                    selectedDateVariable,
                                                                    selectedMedicalPracticeIdVariable,
                                                                    gridSizeVariable,
                                                                    new List<AggregateIdentifier>(),
                                                                    50,
                                                                    appointmentGridViewModelBuilder,
																	errorCallback);


            var changeConfirmationViewModel = new ChangeConfirmationViewModel(viewModelCommunication);
            var undoRedoViewModel = new UndoRedoViewModel(viewModelCommunication,
                                                          appointmentModificationsVariable,
                                                          session);

            var overviewPageViewModel = new OverviewPageViewModel(viewModelCommunication,
                                                                  dateDisplayViewModel,
                                                                  medicalPracticeSelectorViewModel,
                                                                  roomSelectorViewModel,
                                                                  dateSelectorViewModel,
                                                                  gridContainerViewModel,
                                                                  changeConfirmationViewModel,
                                                                  undoRedoViewModel,
                                                                  addAppointmentDialogWindowBuilder,
                                                                  appointmentModificationsVariable,
																  errorCallback);

//            var patientSelectorViewModel = new PatientSelectorViewModel(dataCenter,
//                                                                        selectedPatientForAppointmentSearchVariable);
//
//            var searchPageViewModel = new SearchPageViewModel(patientSelectorViewModel,
//                                                              selectedPatientForAppointmentSearchVariable,
//                                                              selectedDateVariable,
//                                                              selectedMedicalPracticeIdVariable,                                                              
//                                                              viewModelCommunication,
//                                                              dataCenter,
//															  session);

            var optionsPageViewModel = new OptionsPageViewModel();            

            var mainViewModel = new ViewModels.MainView.MainViewModel(overviewPageViewModel,
                                                                      null, // searchPageViewModel,
                                                                      optionsPageViewModel);

            viewModelCommunication.RegisterViewModelMessageHandler<ShowPage>(mainViewModel);
			viewModelCommunication.RegisterViewModelMessageHandler(confirmChangesMessageHandler);
			viewModelCommunication.RegisterViewModelMessageHandler(rejectChangesMessageHandler);

			return mainViewModel;
        }

        public void DisposeViewModel(IMainViewModel viewModelToDispose)
        {
			viewModelCommunication.RemoveViewModelCollection(Constants.TherapyPlaceRowViewModelCollection);
			viewModelCommunication.RemoveViewModelCollection(Constants.AppointmentGridViewModelCollection);
			viewModelCommunication.RemoveViewModelCollection(Constants.TimeGridViewModelCollection);
			viewModelCommunication.RemoveViewModelCollection(Constants.AppointmentViewModelCollection);
	
			viewModelCommunication.DeregisterViewModelMessageHandler<ShowPage>(viewModelToDispose);
			viewModelCommunication.DeregisterViewModelMessageHandler(confirmChangesMessageHandler);
			viewModelCommunication.DeregisterViewModelMessageHandler(rejectChangesMessageHandler);

			var optionsPageViewModel             = viewModelToDispose.OptionsPageViewModel;
	        var searchPageViewModel              = viewModelToDispose.SearchPageViewModel;
	        var overviewPageViewModel            = viewModelToDispose.OverviewPageViewModel;
	        var patientSelectorViewModel         = searchPageViewModel.PatientSelectorViewModel;
	        var dateDisplayViewModel             = overviewPageViewModel.DateDisplayViewModel;
	        var medicalPracticeSelectorViewModel = overviewPageViewModel.MedicalPracticeSelectorViewModel;
	        var roomSelectorViewModel            = overviewPageViewModel.RoomFilterViewModel;
	        var dateSelectorViewModel            = overviewPageViewModel.DateSelectorViewModel;
	        var gridContainerViewModel           = overviewPageViewModel.GridContainerViewModel;
	        var changeConfirmationViewModel      = overviewPageViewModel.ChangeConfirmationViewModel;
	        var undoRedoViewModel                = overviewPageViewModel.UndoRedoViewModel;

	        optionsPageViewModel.Dispose();
			searchPageViewModel.Dispose();
			overviewPageViewModel.Dispose();
			patientSelectorViewModel.Dispose();
			dateDisplayViewModel.Dispose();
			medicalPracticeSelectorViewModel.Dispose();
			roomSelectorViewModel.Dispose();
			dateSelectorViewModel.Dispose();
			gridContainerViewModel.Dispose();
			changeConfirmationViewModel.Dispose();
			undoRedoViewModel.Dispose();

		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
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
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.SearchPage;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel
{
	internal class MainViewModelBuilder : IMainViewModelBuilder
    {
        private readonly IDataCenter dataCenter;
        private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISession session;		
        private readonly AdornerControl adornerControl;

		private ConfirmChangesMessageHandler confirmChangesMessageHandler;
		private RejectChangesMessageHandler rejectChangesMessageHandler;

		public MainViewModelBuilder(IDataCenter dataCenter,
                                    IViewModelCommunication viewModelCommunication,
									ISession session,                                   
                                    AdornerControl adornerControl)
        {
            this.dataCenter = dataCenter;
            this.viewModelCommunication = viewModelCommunication;
	        this.session = session;	        
            this.adornerControl = adornerControl;
        }



        public IMainViewModel Build()
        {            
            // Register Global ViewModelVariables

            var initialMedicalPractice = dataCenter.GetAllMedicalPractices().First();  // TODO set last usage

            var gridSizeVariable                  = new SharedState<Size>(new Size(400, 400));
            var selectedDateVariable              = new SharedState<Date>(initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday());     // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
            var selectedMedicalPracticeIdVariable = new SharedState<Guid>(initialMedicalPractice.Id);
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

            var appointmentModificationsBuilder = new AppointmentModificationsBuilder(dataCenter,
                                                                                      viewModelCommunication,
                                                                                      selectedDateVariable,
                                                                                      gridSizeVariable);

            var appointmentViewModelBuilder = new AppointmentViewModelBuilder(viewModelCommunication,
                                                                              appointmentModificationsVariable,
                                                                              selectedDateVariable,
                                                                              adornerControl,
                                                                              appointmentModificationsBuilder);

            var therapyPlaceRowViewModelBuilder = new TherapyPlaceRowViewModelBuilder(viewModelCommunication,
                                                                                      dataCenter,
                                                                                      adornerControl,
                                                                                      appointmentModificationsVariable,
                                                                                      selectedDateVariable,
                                                                                      selectedMedicalPracticeIdVariable);

            var appointmentGridViewModelBuilder = new AppointmentGridViewModelBuilder(dataCenter,
																					  session,
                                                                                      viewModelCommunication,                                                                                      
                                                                                      gridSizeVariable,
                                                                                      roomFilterVariable,
                                                                                      appointmentModificationsVariable,
                                                                                      appointmentViewModelBuilder,
                                                                                      therapyPlaceRowViewModelBuilder);

            var addAppointmentDialogWindowBuilder = new AddAppointmentDialogWindowBuilder(dataCenter,
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

            var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(dataCenter,
                                                                                        selectedMedicalPracticeIdVariable,
                                                                                        appointmentModificationsVariable);

            var roomSelectorViewModel = new RoomFilterViewModel(dataCenter,
                                                                roomFilterVariable,
                                                                selectedDateVariable,
                                                                selectedMedicalPracticeIdVariable);

            var dateSelectorViewModel = new DateSelectorViewModel(selectedDateVariable);

            var gridContainerViewModel = new GridContainerViewModel(viewModelCommunication,
                                                                    selectedDateVariable,
                                                                    selectedMedicalPracticeIdVariable,
                                                                    gridSizeVariable,
                                                                    new List<AggregateIdentifier>(),
                                                                    50,
                                                                    appointmentGridViewModelBuilder);


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
                                                                  appointmentModificationsVariable);

            var patientSelectorViewModel = new PatientSelectorViewModel(dataCenter,
                                                                        selectedPatientForAppointmentSearchVariable);

            var searchPageViewModel = new SearchPageViewModel(patientSelectorViewModel,
                                                              selectedPatientForAppointmentSearchVariable,
                                                              selectedDateVariable,
                                                              selectedMedicalPracticeIdVariable,                                                              
                                                              viewModelCommunication,
                                                              dataCenter,
															  session);

            var optionsPageViewModel = new OptionsPageViewModel();            

            var mainViewModel = new ViewModels.MainView.MainViewModel(overviewPageViewModel,
                                                                      searchPageViewModel,
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

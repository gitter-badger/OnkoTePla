using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.AppointmentModification;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.Model;
using bytePassion.OnkoTePla.Client.WpfUi.ServiceModules;
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
using bytePassion.OnkoTePla.Core.CommandSystem;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Readmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel
{
    internal class MainViewModelBuilder : IMainViewModelBuilder
    {
        private readonly IDataCenter dataCenter;
        private readonly IViewModelCommunication viewModelCommunication;
        private readonly ICommandBus commandBus;
        private readonly SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory;
        private readonly AdornerControl adornerControl;

        public MainViewModelBuilder(IDataCenter dataCenter,
                                    IViewModelCommunication viewModelCommunication,
                                    ICommandBus commandBus,
                                    SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory,
                                    AdornerControl adornerControl)
        {
            this.dataCenter = dataCenter;
            this.viewModelCommunication = viewModelCommunication;
            this.commandBus = commandBus;
            this.sessionAndUserSpecificEventHistory = sessionAndUserSpecificEventHistory;
            this.adornerControl = adornerControl;
        }

        public IMainViewModel Build()
        {            
            // Register Global ViewModelVariables

            var initialMedicalPractice = dataCenter.Configuration.GetAllMedicalPractices().First();  // TODO set last usage



            var gridSizeVariable = new GlobalState<Size>(new Size(400, 400));
            var selectedDateVariable = new GlobalState<Date>(initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday());     // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
            var selectedMedicalPracticeIdVariable = new GlobalState<Guid>(initialMedicalPractice.Id);
            var roomFilterVariable = new GlobalState<Guid?>();
            var appointmentModificationsVariable = new GlobalState<AppointmentModifications>();

            var selectedPatientForAppointmentSearchVariable = new GlobalState<Patient>();


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
                                                                                      viewModelCommunication,
                                                                                      commandBus,
                                                                                      gridSizeVariable,
                                                                                      roomFilterVariable,
                                                                                      appointmentModificationsVariable,
                                                                                      appointmentViewModelBuilder,
                                                                                      therapyPlaceRowViewModelBuilder);

            var addAppointmentDialogWindowBuilder = new AddAppointmentDialogWindowBuilder(dataCenter,
                                                                                          selectedMedicalPracticeIdVariable,
                                                                                          selectedDateVariable,
                                                                                          appointmentViewModelBuilder);

            // register stand-alone viewModelMessageHandler

            viewModelCommunication.RegisterViewModelMessageHandler(new ConfirmChangesMessageHandler(viewModelCommunication,
                                                                                                    appointmentModificationsVariable,
                                                                                                    selectedMedicalPracticeIdVariable));

            viewModelCommunication.RegisterViewModelMessageHandler(new RejectChangesMessageHandler(viewModelCommunication,
                                                                                                   appointmentModificationsVariable));

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
                                                          sessionAndUserSpecificEventHistory);

            var overviewPageViewModel = new OverviewPageViewModel(dateDisplayViewModel,
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
                                                                commandBus,
                                                                viewModelCommunication,
                                                                dataCenter);
            var optionsPageViewModel = new OptionsPageViewModel();            

            var mainViewModel = new ViewModels.MainView.MainViewModel(overviewPageViewModel,
                                                                      searchPageViewModel,
                                                                      optionsPageViewModel);

            viewModelCommunication.RegisterViewModelMessageHandler<ShowPage>(mainViewModel);

            return mainViewModel;
        }

        public void DisposeViewModel(IMainViewModel viewModelToDispose)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IMainViewModelBuilder
    {
        IMainViewModel Build();
        void DisposeViewModel(IMainViewModel viewModelToDispose);
    }
}

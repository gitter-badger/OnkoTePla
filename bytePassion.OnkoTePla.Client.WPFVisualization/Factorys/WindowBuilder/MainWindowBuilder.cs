using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.ViewModelBuilder;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.WindowBuilder
{
    public class MainWindowBuilder : IWindowBuilder<MainWindow>
	{		
		private readonly IDataCenter dataCenter;
		private readonly ICommandBus commandBus;
		private readonly SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory;
		
		public MainWindowBuilder(IDataCenter dataCenter, 
								 ICommandBus commandBus,
								 SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory)
		{			
			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			this.sessionAndUserSpecificEventHistory = sessionAndUserSpecificEventHistory;
		}

		public MainWindow BuildWindow()
		{
			// initiate ViewModelCommunication			

			IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
			IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);			
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,																						
																						viewModelCollections);

			// Register Global ViewModelVariables

			var initialMedicalPractice = dataCenter.Configuration.GetAllMedicalPractices().First();  // TODO set last usage
			
			
						
            var gridSizeVariable                  = new GlobalState<Size> (new Size(400, 400));
            var selectedDateVariable              = new GlobalState<Date> (initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday());     // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
            var selectedMedicalPracticeIdVariable = new GlobalState<Guid> (initialMedicalPractice.Id);
            var roomFilterVariable                = new GlobalState<Guid?>();			
			var appointmentModificationsVariable  = new GlobalState<AppointmentModifications>();

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

			// build modules

			var adornerControl = new AdornerControl();

			// build factorys

			var appointmentGridViewModelBuilder = new AppointmentGridViewModelBuilder(dataCenter,
																					  viewModelCommunication, 
																					  commandBus, 
																					  adornerControl, 
																					  gridSizeVariable,
																					  roomFilterVariable, 
																					  selectedDateVariable, 
																					  appointmentModificationsVariable,
																					  selectedMedicalPracticeIdVariable);            

            // register stand-alone viewModelMessageHandler

            viewModelCommunication.RegisterViewModelMessageHandler(new ConfirmChangesMessageHandler(viewModelCommunication, 
                                                                                                    appointmentModificationsVariable, 
                                                                                                    selectedMedicalPracticeIdVariable));

			viewModelCommunication.RegisterViewModelMessageHandler(new RejectChangesMessageHandler(viewModelCommunication,
                                                                                                   appointmentModificationsVariable));

			// build factories

			var dialogBuilder = new AddAppointmentDialogWindowBuilder(dataCenter,
																	  viewModelCommunication,
																	  selectedMedicalPracticeIdVariable,
																	  appointmentModificationsVariable,
																	  selectedDateVariable,
																	  gridSizeVariable,
																	  adornerControl);

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
																  dialogBuilder,
                                                                  appointmentModificationsVariable);		            
            
			var patientSelectorViewModel = new PatientSelectorViewModel(dataCenter, 
                                                                        selectedPatientForAppointmentSearchVariable);

			var searchPageViewModel   = new SearchPageViewModel(patientSelectorViewModel, 
																selectedPatientForAppointmentSearchVariable, 
																selectedDateVariable,
																selectedMedicalPracticeIdVariable,
																commandBus,
																viewModelCommunication, 
																dataCenter);
			var optionsPageViewModel  = new OptionsPageViewModel();

			var notificationServiceContainerViewModel = new NotificationServiceContainerViewModel(viewModelCommunication);

			var mainWindowViewModel = new MainWindowViewModel(overviewPageViewModel,
																 searchPageViewModel,
																 optionsPageViewModel,
																 notificationServiceContainerViewModel);

			viewModelCommunication.RegisterViewModelMessageHandler<ShowPage>(mainWindowViewModel);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

            // set GridContainer as ReferenceElement of AdornerControl

		    adornerControl.ReferenceElement = mainWindow.OverviewPage.GridContainer;

		    return mainWindow;
		}

		public void DisposeWindow(MainWindow buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

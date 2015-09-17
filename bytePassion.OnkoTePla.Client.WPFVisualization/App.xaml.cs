using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization
{

	public partial class App
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			base.OnStartup(e);

			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////                          Composition Root and Setup                         //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////


			// Patient-Repository

			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
			IPatientReadRepository patientReadRepository = new PatientRepository(patientPersistenceService);
			patientReadRepository.LoadRepository();


			// Config-Repository

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			IConfigurationReadRepository configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();


			// EventStore

			IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> eventStorePersistenceService = new XmlEventStreamDataStore(GlobalConstants.EventHistoryPersistenceFile);
			IEventStore eventStore = new EventStore(eventStorePersistenceService, configReadRepository);
			eventStore.LoadRepository(); 


			// Event- and CommandBus

			IHandlerCollection<DomainEvent>   eventHandlerCollection   = new MultiHandlerCollection <DomainEvent>();
			IHandlerCollection<DomainCommand> commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();

			IMessageBus<DomainEvent>   eventMessageBus   = new LocalMessageBus<DomainEvent>  (eventHandlerCollection);			
			IMessageBus<DomainCommand> commandMessageBus = new LocalMessageBus<DomainCommand>(commandHandlerCollection);

			IEventBus   eventBus   = new EventBus(eventMessageBus);
			ICommandBus commandBus = new CommandBus(commandMessageBus);


			// Aggregate- and Readmodel-Repositories

			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientReadRepository, configReadRepository);
			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientReadRepository, configReadRepository);


			// Register CommandHandler

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));
			commandBus.RegisterCommandHandler(new DeleteAppointmentCommandHandler(aggregateRepository));


			// SessionInformation

			var sessionInformation = new SessionInformation
			{
				LoggedInUser = configReadRepository.GetAllUsers().First()
			};


			// Data-Model

			IDataCenter dataCenter = new DataCenter(configReadRepository, 
													patientReadRepository, 
													readModelRepository, 
													sessionInformation);


			// initiate ViewModelCommunication			

			IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
			IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
			IStateEngine viewModelStateEngine = new StateEngine();
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
																	                    viewModelStateEngine,
																	                    viewModelCollections);


			// Register Global ViewModelVariables

			var initialMedicalPractice = configReadRepository.GetAllMedicalPractices().First();  // TODO set last usage

			var gridSizeInitialValue          = new Size(400,400);
			var selectedDateInitialValue      = initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday();
			var displayedPracticeInitialValue = initialMedicalPractice.Id; // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört	

			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSizeVariable,              gridSizeInitialValue);
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSelectedDateVariable,      selectedDateInitialValue);
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridDisplayedPracticeVariable, displayedPracticeInitialValue);		// TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridRoomFilterVariable,        (Guid?) null);							// when selectedRoomID == null --> all rooms are selected
			viewModelCommunication.RegisterGlobalViewModelVariable(SideBarStateVariable,                     true);									// true --> full width; false --> minimized
			viewModelCommunication.RegisterGlobalViewModelVariable(CurrentModifiedAppointmentVariable,       (AppointmentModifications)null);		// null -> no appointment selected


			// Create ViewModelCollection

			viewModelCommunication.CreateViewModelCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				TherapyPlaceRowViewModelCollection				
			);

			viewModelCommunication.CreateViewModelCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection				
			);

			viewModelCommunication.CreateViewModelCollection<ITimeGridViewModel, AggregateIdentifier>(
				TimeGridViewModelCollection				
			);

			viewModelCommunication.CreateViewModelCollection<IAppointmentViewModel, Guid>(
				AppointmentViewModelCollection				
			);


			// register stand-alone viewModelMessageHandler

			viewModelCommunication.RegisterViewModelMessageHandler(new ConfirmChangesMessageHandler(viewModelCommunication));
			viewModelCommunication.RegisterViewModelMessageHandler(new RejectChangesMessageHandler(viewModelCommunication));


			// create permanent ViewModels

			var dateDisplayViewModel = new DateDisplayViewModel(viewModelCommunication);
			var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(dataCenter, viewModelCommunication);
			var roomSelectorViewModel = new RoomFilterViewModel(dataCenter, viewModelCommunication);
			var dateSelectorViewModel = new DateSelectorViewModel(viewModelCommunication);
			var gridContainerViewModel = new GridContainerViewModel(dataCenter,
																	commandBus,
																	viewModelCommunication,                                                                    
																	new List<AggregateIdentifier>(), 
																	50);

			var changeConfirmationViewModel = new ChangeConfirmationViewModel(viewModelCommunication);
			
			var overviewPageViewModel = new OverviewPageViewModel(dateDisplayViewModel,
																  medicalPracticeSelectorViewModel, 
																  roomSelectorViewModel, 
																  dateSelectorViewModel, 
																  gridContainerViewModel,
																  changeConfirmationViewModel,
																  viewModelCommunication);

			var searchPageViewModel   = new SearchPageViewModel();
			var optionsPageViewModel  = new OptionsPageViewModel();


			var newMainWindowViewModel = new NewMainWindowViewModel(overviewPageViewModel, 
																	searchPageViewModel, 
																	optionsPageViewModel,
																	viewModelCommunication);

			var mainWindow = new NewMainWindow
			{
				DataContext = newMainWindowViewModel
			};

			mainWindow.ShowDialog();


			///////////////////////////////////////////////////////////////////////////////////////////////
			////////                                                                             //////////
			////////             Clean Up and store data after main Window was closed            //////////
			////////                                                                             //////////
			///////////////////////////////////////////////////////////////////////////////////////////////


			eventStore.PersistRepository();
		}		
	}
}

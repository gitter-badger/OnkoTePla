using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
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
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
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

			var initialMedicalPractice = configReadRepository.GetAllMedicalPractices().First();  // TODO set last usage


			IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
			IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
			IStateEngine viewModelStateEngine = new StateEngine();
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			var viewModelCommunication = new ViewModelCommunication<ViewModelMessage>(viewModelMessageBus,
																					  viewModelStateEngine,
																					  viewModelCollections);
			
			var gridSizeInitialValue          = new Size(400,400);
			var selectedDateInitialValue      = initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday();
			var displayedPracticeInitialValue = initialMedicalPractice.Id; // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört			


			// Register Global ViewModelVariables

			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSizeVariable, gridSizeInitialValue);


			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSelectedDateVariable,      selectedDateInitialValue);
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridDisplayedPracticeVariable, displayedPracticeInitialValue); // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridRoomFilterVariable,      (Guid?) null);                  // when selectedRoomID == null --> all rooms are selected
			viewModelCommunication.RegisterGlobalViewModelVariable(SideBarStateVariable,                     true);                          // true --> full width; false --> minimized


			// Create ViewModelCollection

			viewModelCommunication.CreateViewModelCollection<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				TherapyPlaceRowViewModelCollection,
				(viewModel, therapyPlaceLocalisation) => viewModel.Identifier == therapyPlaceLocalisation
			);

			viewModelCommunication.CreateViewModelCollection<AppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection,
				(viewModel, identifier) =>  viewModel.Identifier == identifier
			);

			viewModelCommunication.CreateViewModelCollection<TimeGridViewModel, AggregateIdentifier>(
				TimeGridViewModelCollection,
				(viewModel, identifier) => viewModel.Identifier == identifier
			);

			viewModelCommunication.CreateViewModelCollection<AppointmentViewModel, Guid>(
				AppointmentViewModelCollection,
				(viewModel, id) => viewModel.Identifier == id
			);



			// create permanent ViewModels
		
			var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(dataCenter, viewModelCommunication);
			var roomSelectorViewModel = new RoomFilterViewModel(dataCenter, viewModelCommunication);
			var dateSelectorViewModel = new DateSelectorViewModel(viewModelCommunication);
			var gridContainerViewModel = new GridContainerViewModel(dataCenter,
																	viewModelCommunication,                                                                    
																	new List<AggregateIdentifier>(), 
																	50);

			var overviewPageViewModel = new OverviewPageViewModel(medicalPracticeSelectorViewModel, 
																  roomSelectorViewModel, 
																  dateSelectorViewModel, 
																  gridContainerViewModel);

			var searchPageViewModel   = new SearchPageViewModel();
			var optionsPageViewModel  = new OptionsPageViewModel();


			var newMainWindowViewModel = new NewMainWindowViewModel(overviewPageViewModel, searchPageViewModel, optionsPageViewModel);

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


			//eventStore.PersistRepository();
		}		
	}
}

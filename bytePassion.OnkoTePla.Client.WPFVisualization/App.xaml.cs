using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;

using static bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess.Global;


namespace bytePassion.OnkoTePla.Client.WPFVisualization
{

	public partial class App //: Application
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
			IPatientReadRepository patientRepository = new PatientRepository(patientPersistenceService);
			patientRepository.LoadRepository();


			// Config-Repository

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			IConfigurationReadRepository configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();


			// EventStore

//			IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> eventStorePersistenceService = new XmlEventStreamDataStore(GlobalConstants.EventHistoryPersistenceFile);
//			IEventStore eventStore = new EventStore(eventStorePersistenceService, configReadRepository);
//			eventStore.LoadRepository(); 


			// Event- and CommandBus

//			IHandlerCollection<DomainEvent>   eventHandlerCollection   = new MultiHandlerCollection <DomainEvent>();
//			IHandlerCollection<DomainCommand> commandHandlerCollection = new SingleHandlerCollection<DomainCommand>();
//
//			IMessageBus<DomainEvent>   eventMessageBus   = new LocalMessageBus<DomainEvent>  (eventHandlerCollection);			
//			IMessageBus<DomainCommand> commandMessageBus = new LocalMessageBus<DomainCommand>(commandHandlerCollection);
//
//			IEventBus   eventBus   = new EventBus(eventMessageBus);
//			ICommandBus commandBus = new CommandBus(commandMessageBus);


			// Aggregate- and Readmodel-Repositories

//			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientRepository, configReadRepository);
//			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientRepository, configReadRepository);


			// Register CommandHandler

//			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));
//			commandBus.RegisterCommandHandler(new DeleteAppointmentCommandHandler(aggregateRepository));


			// SessionInformation

//			var sessionInformation = new SessionInformation
//			{
//				LoggedInUser = configReadRepository.GetAllUsers().First()
//			};

			// GlobalStates registration

			var initialMedicalPractice = configReadRepository.GetAllMedicalPractices().First();  // TODO set last usage


			IHandlerCollection<ViewModelMessageBase> handlerCollection = new MultiHandlerCollection<ViewModelMessageBase>();
			IMessageBus<ViewModelMessageBase> viewModelMessageBus = new LocalMessageBus<ViewModelMessageBase>(handlerCollection);
			IStateEngine viewModelStateEngine = new StateEngine();
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			ViewModelCommunication = new ViewModelCommunication<ViewModelMessageBase>(viewModelMessageBus,
																					  viewModelStateEngine,
																					  viewModelCollections);

			var        gridSizeInitialValue          = new Size(400,400);
			var        selectedDateInitialValue      = initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday();
			var        displayedPracticeInitialValue = new Tuple<Guid, uint>(initialMedicalPractice.Id, initialMedicalPractice.Version); // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört			


			ViewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSizeVariable, gridSizeInitialValue);


			ViewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSelectedDateVariable,      selectedDateInitialValue);
			ViewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridDisplayedPracticeVariable, displayedPracticeInitialValue); // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
			ViewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSelectedRoomVariable,      (Guid?) null);                  // when selectedRoomID == null --> all rooms are selected
			ViewModelCommunication.RegisterGlobalViewModelVariable(SideBarStateVariable,                     true);                          // true --> full width; false --> minimized


			ViewModelCommunication.CreateViewModelCollection<TherapyPlaceRowViewModel, AppointmentLocalisation>(
				TherapyPlaceRowViewModelCollection,
				(viewModel, therapyPlaceLocalisation) => viewModel.LocalisationIdentifier == therapyPlaceLocalisation
			);



			// create permanent ViewModels

//			var addAppointmentTestViewModel      = new AddAppointmentTestViewModel(configReadRepository, patientRepository, readModelRepository, commandBus);
//			var appointmentOverViewModel         = new AppointmentOverViewModel(readModelRepository, configReadRepository);
//            var patientsViewModel                = new PatientSelectorViewModel(patientRepository);
//			var appointmentGridViewModel         = new AppointmentGridViewModel(readModelRepository, configReadRepository, commandBus, sessionInformation);
//			var dateSelectorViewModel            = new DateSelectorViewModel(configReadRepository);
//			var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(configReadRepository);
//			var roomSelectorViewModel            = new RoomSelectorViewModel(configReadRepository);
//
//			var mainWindowViewModel = new MainWindowViewModel(patientsViewModel, 
//															  addAppointmentTestViewModel, 
//															  appointmentOverViewModel, 
//															  appointmentGridViewModel,
//															  dateSelectorViewModel,
//															  medicalPracticeSelectorViewModel,
//															  roomSelectorViewModel,
//															  sessionInformation);

			// create and show main Window

//			var mainWindow = new MainWindow
//			{
//				DataContext = mainWindowViewModel
//			};			


			var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(configReadRepository);
			var roomSelectorViewModel = new RoomSelectorViewModel(configReadRepository);
			var dateSelectorViewModel = new DateSelectorViewModel(configReadRepository);
			var gridContainerViewModel = new GridContainerViewModel();

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

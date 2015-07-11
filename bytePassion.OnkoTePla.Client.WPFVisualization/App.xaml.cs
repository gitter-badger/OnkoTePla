using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.CommandHandler;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Aggregate;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.Core.State;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization
{
	
	public partial class App : Application
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

			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new JSonPatientDataStore(GlobalConstants.PatientJSONPersistenceFile);
			IPatientReadRepository patientRepository = new PatientRepository(patientPersistenceService);
			patientRepository.LoadRepository();


			// Config-Repository

			IPersistenceService<Configuration> configPersistenceService = new XmlConfigurationDataStore(GlobalConstants.ConfigPersistenceFile);
			IConfigurationReadRepository configReadRepository = new ConfigurationRepository(configPersistenceService);
			configReadRepository.LoadRepository();


			// EventStore

			IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>> eventStorePersistenceService = new XmlEventStreamDataStore(GlobalConstants.EventHistoryPersistenceFile);
			IEventStore eventStore = new EventStore(eventStorePersistenceService, configReadRepository);
			eventStore.LoadRepository(); 


			// Event- and CommandBus

			IEventBus   eventBus   = new EventBus();
			ICommandBus commandBus = new CommandBus();


			// Aggregate- and Readmodel-Repositories

			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientRepository, configReadRepository);
			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientRepository, configReadRepository);


			// Register CommandHandler

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));
			commandBus.RegisterCommandHandler(new DeleteAppointmentCommandHandler(aggregateRepository));


			// SessionInformation

			var sessionInformation = new SessionInformation
			{
				LoggedInUser = configReadRepository.GetAllUsers().First()
			};

			// GlobalStates

			IStateEngine stateEngine = new StateEngine();

			stateEngine.RegisterState<Date>(GlobalConstants.GlobalStateMainGridSelectedDate);

			var mainGridSelectedDateState = stateEngine.GetState<Date>(GlobalConstants.GlobalStateMainGridSelectedDate);

			// create permanent ViewModels

			var addAppointmentTestViewModel = new AddAppointmentTestViewModel(configReadRepository, patientRepository, readModelRepository, commandBus);
			var appointmentOverViewModel    = new AppointmentOverViewModel(readModelRepository, configReadRepository);
            var patientsViewModel           = new PatientSelectorViewModel(patientRepository);
			var appointmentGridViewModel    = new AppointmentGridViewModel(readModelRepository, configReadRepository, commandBus, 
																		   sessionInformation, mainGridSelectedDateState); 		

			var mainWindowViewModel = new MainWindowViewModel(patientsViewModel, 
															  addAppointmentTestViewModel, 
															  appointmentOverViewModel, 
															  appointmentGridViewModel,
															  sessionInformation);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
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

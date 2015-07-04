using System.Collections.Generic;
using System.Windows;
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
using bytePassion.OnkoTePla.Client.Resources;
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

			IPersistenceService<IEnumerable<Patient>> patientPersistenceService = new XmlPatientDataStore(GlobalConstants.PatientPersistenceFile);
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

			IEventBus eventBus = new EventBus();
			ICommandBus commandBus = new CommandBus();


			// Aggregate- and Readmodel-Repositories

			IAggregateRepository aggregateRepository = new AggregateRepository(eventBus, eventStore, patientRepository, configReadRepository);
			IReadModelRepository readModelRepository = new ReadModelRepository(eventBus, eventStore, patientRepository, configReadRepository);


			// Register CommandHandler

			commandBus.RegisterCommandHandler(new AddAppointmentCommandHandler(aggregateRepository));


			// create ViewModels

			var addAppointmentTestViewModel = new AddAppointmentTestViewModel(configReadRepository, patientRepository, readModelRepository, commandBus);
			var appointmentOverViewModel    = new AppointmentOverViewModel(readModelRepository, configReadRepository);
            var patientsViewModel           = new PatientSelectorViewModel(patientRepository);			
			var appointmentGridViewModel    = new AppointmentGridViewModel(readModelRepository, configReadRepository); 																		

			var mainWindowViewModel = new MainWindowViewModel(patientsViewModel, 
															  addAppointmentTestViewModel, 
															  appointmentOverViewModel, 
															  appointmentGridViewModel);

			var mainWindow = new MainWindow
			{
				DataContext = mainWindowViewModel
			};

			//			// TODO: justForTesting ////////////////////////////////////////////
			//
			//			addAppointmentTestViewModel.SelectedDateAsString = "3.7.2015";
			//			addAppointmentTestViewModel.SelectedMedicalPractice = configReadRepository.GetAllMedicalPractices().First();
			//			addAppointmentTestViewModel.SelectedUser = configReadRepository.GetAllUsers().First();
			//			addAppointmentTestViewModel.Description = "blubb234";
			//			addAppointmentTestViewModel.StartTimeAsString = "10:00";
			//			addAppointmentTestViewModel.EndTimeAsString = "14:00";
			//
			//			addAppointmentTestViewModel.LoadReadModel.Execute(null);
			//
			//			////////////////////////////////////////////////////////////////////
			//
			//			appointmentOverViewModel.SelectedDateAsString = "3.7.2015";
			//			appointmentOverViewModel.SelectedMedicalPractice = configReadRepository.GetAllMedicalPractices().First();
			//
			//			////////////////////////////////////////////////////////////////////

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

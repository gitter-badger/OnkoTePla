using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AddAppointmentTestViewModel : IAddAppointmentTestViewModel
	{
		private readonly IConfigurationReadRepository config;
		private readonly IPatientReadRepository patients;


		// ReSharper disable NotAccessedField.Local
		private readonly ICommandBus commandBus;		
		private readonly IReadModelRepository readModelRepository;
		// ReSharper restore NotAccessedField.Local


		private AppointmentsOfADayReadModel readModel;
		private IEnumerable<TherapyPlace> therapyPlaces; 

		private readonly Command loadReadModelCommand;
		private readonly Command addAppointmentCommmand;

		public AddAppointmentTestViewModel(IConfigurationReadRepository config, 
										   IPatientReadRepository patients, 
										   IReadModelRepository readModelRepository,
										   ICommandBus commandBus )
		{
			this.config = config;
			this.patients = patients;
			this.commandBus = commandBus;
			this.readModelRepository = readModelRepository;

			loadReadModelCommand = new Command(
				() =>
				{
					var date = Date.Parse(SelectedDateAsString);
					var identifier = new AggregateIdentifier(date, SelectedMedicalPractice.Id);
					readModel = readModelRepository.GetAppointmentsOfADayReadModel(identifier);

					therapyPlaces = config.GetMedicalPracticeByIdAndVersion(readModel.Identifier.MedicalPracticeId,readModel.Identifier.PracticeVersion)
										  .GetAllTherapyPlaces();					
				});

			addAppointmentCommmand = new Command(
				() =>
				{
					commandBus.Send(new AddAppointment(readModel.Identifier, readModel.AggregateVersion, 
													   SelectedUser.Id, 
													   SelectedPatient.Id, 
													   Description, 
													   Time.Parse(StartTimeAsString), Time.Parse(EndTimeAsString), 
													   SelectedTherapyPlace.Id));
				});
		}

		public IEnumerable<MedicalPractice> MedicalPractices
		{
			get { return config.GetAllMedicalPractices(); }
		}

		public IEnumerable<User> Users
		{
			get { return config.GetAllUsers(); }
		}

		public IEnumerable<Patient> Patients
		{
			get { return patients.GetAllPatients(); }
		}

		public IEnumerable<TherapyPlace> TherapyPlaces
		{
			get { return therapyPlaces; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref therapyPlaces, value);
			}
		}

		public MedicalPractice SelectedMedicalPractice { get; set; }
		public string          SelectedDateAsString    { get; set; }
		public User            SelectedUser            { get; set; }
		public Patient         SelectedPatient         { get; set; }
		public string          Description             { get; set; }
		public string          StartTimeAsString       { get; set; }
		public string          EndTimeAsString         { get; set; }
		public TherapyPlace    SelectedTherapyPlace    { get; set; }

		public ICommand LoadReadModel  { get { return loadReadModelCommand;   }}
		public ICommand AddAppointment { get { return addAppointmentCommmand; }}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

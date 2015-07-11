using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.Bus;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentGridViewModel : IAppointmentGridViewModel
	{

		// FrameworkAccess /////////////////////////////////////////////////////////////////////////////////

		private readonly IReadModelRepository         readModelRepository;
		private readonly IConfigurationReadRepository configuration;
		private readonly ICommandBus                  commandBus;
		private readonly SessionInformation           sessionInformation;

		// GridDrawing /////////////////////////////////////////////////////////////////////////////////////

		private double currentGridWidth;
		private double currentGridHeight;
		
		// AppointmentData /////////////////////////////////////////////////////////////////////////////////

		private readonly IDictionary<AggregateIdentifier, AppointmentGridDisplayDataSet> appointmentDataSets;
		private AppointmentGridDisplayDataSet currentlyDisplayedDataSet;				

		// Commands ////////////////////////////////////////////////////////////////////////////////////////

		private readonly ParameterrizedCommand<AggregateIdentifier> showPracticeAndDateCommand;		

		private readonly Command commitChangesCommand;
		private readonly Command discardChangesCommand;

		// Grid state //////////////////////////////////////////////////////////////////////////////////////

		private IAppointmentViewModel editingObject;
		private OperatingMode operatingMode;


		public AppointmentGridViewModel(IReadModelRepository readModelRepository, 
										IConfigurationReadRepository configuration,
										ICommandBus commandBus, 
										SessionInformation sessionInformation)
		{
			this.readModelRepository = readModelRepository;
			this.configuration = configuration;
			this.commandBus = commandBus;
			this.sessionInformation = sessionInformation;
			
			appointmentDataSets = new Dictionary<AggregateIdentifier, AppointmentGridDisplayDataSet>();
			currentlyDisplayedDataSet = null;
			editingObject = null;
			operatingMode = OperatingMode.View;
			
			showPracticeAndDateCommand = new ParameterrizedCommand<AggregateIdentifier>(ShowPracticeAndDateOnScreen);			
			commitChangesCommand  = new Command(CommitAllChanges);
			discardChangesCommand = new Command(DiscardAllChanges);

			currentGridWidth  = 400;	 // will be overwritten when View is created
			currentGridHeight = 400;	 // but is nessacary for loading todays dataSet

			var medicalPractice = configuration.GetAllMedicalPractices().First();
			var lastOpenDay = GetLastOpenDay(medicalPractice);

			ShowPracticeAndDateOnScreen(new AggregateIdentifier(lastOpenDay, medicalPractice.Id));
		}

		private Date GetLastOpenDay(MedicalPractice medicalPractice)
		{
			var currentDate = TimeTools.Today();

			var securityCounter = 0;

			while (!medicalPractice.HoursOfOpening.IsOpen(currentDate) && (securityCounter++)<1000)
				currentDate = currentDate.DayBefore();

			if (securityCounter > 990)
				throw new ArgumentException();

			return currentDate;
		}

		private void ShowPracticeAndDateOnScreen(AggregateIdentifier identifier)
		{

			AppointmentGridDisplayDataSet appointmentDataSet;

			if (appointmentDataSets.ContainsKey(identifier))
			{
				appointmentDataSet = appointmentDataSets[identifier];
			}
			else
			{
				var appointmentReadModel = readModelRepository.GetAppointmentsOfADayReadModel(identifier);
				var updatedIdentifier = appointmentReadModel.Identifier;
				var medicalPractice = configuration.GetMedicalPracticeByIdAndVersion(updatedIdentifier.MedicalPracticeId,
				                                                                     updatedIdentifier.PracticeVersion);

				appointmentDataSet = new AppointmentGridDisplayDataSet(appointmentReadModel, medicalPractice, this);
				appointmentDataSets.Add(updatedIdentifier, appointmentDataSet);
			}

			currentlyDisplayedDataSet = appointmentDataSet;
			currentlyDisplayedDataSet.SetNewGridHeight(CurrentGridHeight);
			currentlyDisplayedDataSet.SetNewGridWidth(CurrentGridWidth);

			NotifyViewThatNewDataSetIsLoaded();						
		}

		private void NotifyViewThatNewDataSetIsLoaded()
		{
			PropertyChanged.Notify(this, "TimeSlotLabels");
			PropertyChanged.Notify(this, "TimeSlotLines");
			PropertyChanged.Notify(this, "TherapyPlaceRows");
		}


		private void CommitAllChanges()
		{
			// TODO
			OperatingMode = OperatingMode.View;
		}

		private void DiscardAllChanges()
		{
			// TODO
			OperatingMode = OperatingMode.View;
		}
		

		public ICommand ShowPracticeAndDate { get { return showPracticeAndDateCommand; }}
		public ICommand CommitChanges       { get { return commitChangesCommand;       }}
		public ICommand DiscardChanges      { get { return discardChangesCommand;      }}

		
		public void DeleteAppointment(IAppointmentViewModel appointmentViewModel, Appointment appointment, ITherapyPlaceRowViewModel containerRow)
		{
			commandBus.Send(new DeleteAppointment(currentlyDisplayedDataSet.AppointmentReadModel.Identifier, 
												  currentlyDisplayedDataSet.AppointmentReadModel.AggregateVersion, 
												  sessionInformation.LoggedInUser.Id, 
												  appointment.Id, 
												  appointment.Patient.Id));
			
			OperatingMode = OperatingMode.View;
		}

		public ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get { return currentlyDisplayedDataSet.TimeSlotLabels;   }} 
		public ObservableCollection<TimeSlotLine>              TimeSlotLines    { get { return currentlyDisplayedDataSet.TimeSlotLines;    }} 
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get { return currentlyDisplayedDataSet.TherapyPlaceRows; }}

		public double CurrentGridWidth
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridWidth, value);
				currentlyDisplayedDataSet.SetNewGridWidth(currentGridWidth);				
			}
			get { return currentGridWidth; }
		}

		public double CurrentGridHeight
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridHeight, value);				
				currentlyDisplayedDataSet.SetNewGridHeight(currentGridHeight);
			}
			get { return currentGridHeight; }
		}

		public IAppointmentViewModel EditingObject
		{
			get { return editingObject; }
			set
			{				
				// lock day

				OperatingMode = value == null ? OperatingMode.View : OperatingMode.Edit;
				PropertyChanged.ChangeAndNotify(this, ref editingObject, value);
			}
		}

		public OperatingMode OperatingMode
		{
			get { return operatingMode; }
			private set { PropertyChanged.ChangeAndNotify(this, ref operatingMode, value);}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		///////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                           ///////////
		/////////                                  TestArea                                 ///////////
		/////////                                                                           ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////

		public void TestLoad()
		{
			var identifier = new AggregateIdentifier(new Date(3, 7, 2015), configuration.GetMedicalPracticeByName("examplePractice1").Id);
			ShowPracticeAndDate.Execute(identifier);
		}
	}
}

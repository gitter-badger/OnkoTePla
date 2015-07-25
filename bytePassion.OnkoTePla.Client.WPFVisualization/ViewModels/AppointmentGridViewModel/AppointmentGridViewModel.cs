using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.Commands;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel
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

		private readonly GlobalState<Guid?> selectedRoomState;
		private readonly GlobalState<Tuple<Guid, uint>> displayedPracticeState;
		private readonly GlobalState<Date> selectedDateState; 

		public AppointmentGridViewModel(IReadModelRepository readModelRepository, 
										IConfigurationReadRepository configuration,
										ICommandBus commandBus, 
										SessionInformation sessionInformation, 
										GlobalState<Date> selectedDateState,
										GlobalState<Tuple<Guid, uint>> displayedPracticeState,
										GlobalState<Guid?> selectedRoomState )
		{
			this.readModelRepository = readModelRepository;
			this.configuration = configuration;
			this.commandBus = commandBus;
			this.sessionInformation = sessionInformation;
			this.selectedRoomState = selectedRoomState;
			this.displayedPracticeState = displayedPracticeState;
			this.selectedDateState = selectedDateState;

			appointmentDataSets = new Dictionary<AggregateIdentifier, AppointmentGridDisplayDataSet>();
			currentlyDisplayedDataSet = null;
			editingObject = null;
			operatingMode = OperatingMode.View;
			
			showPracticeAndDateCommand = new ParameterrizedCommand<AggregateIdentifier>(ShowPracticeAndDateOnScreen);			
			commitChangesCommand  = new Command(CommitAllChanges);
			discardChangesCommand = new Command(DiscardAllChanges);

			currentGridWidth  = 400;	 // will be overwritten when View is created
			currentGridHeight = 400;	 // but is nessacary for loading intial dataSet						

			selectedDateState.StateChanged      += OnSelectedDateChanged;
			displayedPracticeState.StateChanged += OnDisplayedPracticeChanged;

			ShowPracticeAndDateOnScreen(new AggregateIdentifier(selectedDateState.Value, displayedPracticeState.Value.Item1));
		}

		private void OnDisplayedPracticeChanged(Tuple<Guid, uint> practiceInfo)
		{
			var currentIdentifier = currentlyDisplayedDataSet.AppointmentReadModel.Identifier;

			if (practiceInfo.Item1 != currentIdentifier.MedicalPracticeId)
				ShowPracticeAndDateOnScreen(new AggregateIdentifier(currentIdentifier.Date,
																	practiceInfo.Item1));
		}	

		private void OnSelectedDateChanged(Date date)
		{			
			if (currentlyDisplayedDataSet.AppointmentReadModel.Identifier.Date == date) 
				return;

			var currentIdentifier = currentlyDisplayedDataSet.AppointmentReadModel.Identifier;

			ShowPracticeAndDateOnScreen(new AggregateIdentifier(date, 
																currentIdentifier.MedicalPracticeId));
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

				appointmentDataSet = new AppointmentGridDisplayDataSet(appointmentReadModel, medicalPractice, this, selectedRoomState);
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
			PropertyChanged.Notify(this, "displayedTherapyPlaceRows");
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
			commandBus.SendCommand(new DeleteAppointment(currentlyDisplayedDataSet.AppointmentReadModel.Identifier, 
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
				// TODO lock day

				OperatingMode = value == null ? OperatingMode.View : OperatingMode.Edit;
				PropertyChanged.ChangeAndNotify(this, ref editingObject, value);
			}
		}

		public OperatingMode OperatingMode
		{
			get { return operatingMode; }
			private set { PropertyChanged.ChangeAndNotify(this, ref operatingMode, value);}
		}

		public Date CurrentlyDisplayedDate
		{
			get { return currentlyDisplayedDataSet.AppointmentReadModel.Identifier.Date; }
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


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentGridViewModel : IAppointmentGridViewModel
	{
		// DataAccess //////////////////////////////////////////////////////////////////////////////////////

		private readonly IReadModelRepository         readModelRepository;
		private readonly IConfigurationReadRepository configuration;


		// GridDrawing /////////////////////////////////////////////////////////////////////////////////////

		private double currentGridWidth;
		private double currentGridHeight;

		private Time startTime;
		private Time endTime;

		private readonly ObservableCollection<TimeSlotLabel> timeSlotLabels;
		private readonly ObservableCollection<TimeSlotLine>  timeSlotLines;


		// AppointmentData /////////////////////////////////////////////////////////////////////////////////

		private AppointmentsOfADayReadModel appointmentReadModel;
		
		private readonly ObservableCollection<ITherapyPlaceRowViewModel> therapyPlaceRows;
		private readonly IDictionary<Guid, ObservableCollection<IAppointmentViewModel>> appointmentsOnGrid;		


		// Commands ////////////////////////////////////////////////////////////////////////////////////////

		private readonly ParameterrizedCommand<AggregateIdentifier> loadReamodelCommand; 



		public AppointmentGridViewModel(IReadModelRepository readModelRepository, 
										IConfigurationReadRepository configuration)
		{
			this.readModelRepository = readModelRepository;
			this.configuration = configuration;

			startTime = new Time(7, 0);
			endTime   = new Time(16, 0);
			timeSlotLabels = new ObservableCollection<TimeSlotLabel>();
			timeSlotLines  = new ObservableCollection<TimeSlotLine>();

			appointmentReadModel = null;			
			therapyPlaceRows = new ObservableCollection<ITherapyPlaceRowViewModel>();
			appointmentsOnGrid = new ConcurrentDictionary<Guid, ObservableCollection<IAppointmentViewModel>>();			
			
			loadReamodelCommand = new ParameterrizedCommand<AggregateIdentifier>(LoadReadModelFromId);
		}

		private void OnAppointmentChanged(object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			switch (appointmentChangedEventArgs.ChangeAction)
			{
				case ChangeAction.Added:
				{
					var appointment = appointmentChangedEventArgs.Appointment;
					appointmentsOnGrid[appointment.TherapyPlace.Id].Add(new AppointmentViewModel(appointment));
					break;
				}
			}
		}

		private void LoadReadModelFromId(AggregateIdentifier id)
		{
			if (appointmentReadModel != null)
				appointmentReadModel.AppointmentChanged -= OnAppointmentChanged;

			appointmentReadModel = readModelRepository.GetAppointmentsOfADayReadModel(id);
			appointmentReadModel.AppointmentChanged += OnAppointmentChanged;

			var updatedId = appointmentReadModel.Identifier;

			var medicalPractice = configuration.GetMedicalPracticeByIdAndVersion(updatedId.MedicalPracticeId,
																				 updatedId.PracticeVersion);

			if (!medicalPractice.HoursOfOpening.IsOpen(updatedId.Date))
				throw new ArgumentException();

			startTime = medicalPractice.HoursOfOpening.GetOpeningTime(updatedId.Date);
			endTime   = medicalPractice.HoursOfOpening.GetClosingTime(updatedId.Date);

			therapyPlaceRows.Clear();

			foreach (var room in medicalPractice.Rooms)  
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					var appointmentListForTherapyPlace = new ObservableCollection<IAppointmentViewModel>();
					appointmentsOnGrid.Add(therapyPlace.Id, appointmentListForTherapyPlace);
					therapyPlaceRows.Add(new TherapyPlaceRowViewModel(appointmentListForTherapyPlace, therapyPlace,room.DisplayedColor, startTime, endTime));
				}

			foreach (var appointment in appointmentReadModel.Appointments)			
				appointmentsOnGrid[appointment.TherapyPlace.Id].Add(new AppointmentViewModel(appointment));
			
			RecomputeGrid();
		}

		public ICommand LoadReadModel
		{
			get { return loadReamodelCommand; }
		}

		public ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get { return timeSlotLabels;   }}
		public ObservableCollection<TimeSlotLine>              TimeSlotLines    { get { return timeSlotLines;    }}
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get { return therapyPlaceRows; }}

		public double CurrentGridWidth
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridWidth, value);
				RecomputeGrid();
			}
			get { return currentGridWidth; }
		}

		public double CurrentGridHeight
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridHeight, value);
				RecomputeGrid();
			}
			get { return currentGridHeight; }
		}

		private void RecomputeGrid()
		{
			if (CurrentGridWidth < 200)
				return;

			const uint slotLengthInSeconds = 1800;

			var duration = Time.GetDurationBetween(endTime, startTime);

			var timeSlotCount = duration.Seconds / slotLengthInSeconds;
			var timeSlotWidth = CurrentGridWidth / timeSlotCount;

			timeSlotLabels.Clear();
			timeSlotLines.Clear();

			for (uint slot = 0; slot < timeSlotCount + 1; slot++)
			{

				var timeCaption = new Time(startTime + new Duration(slot*slotLengthInSeconds))
									.ToString()
									.Substring(0, 5);

				timeSlotLabels.Add(new TimeSlotLabel(timeCaption)
								{
									XCoord = slot * timeSlotWidth,
									YCoord = 30
								});

				timeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = slot * timeSlotWidth,
					YCoordTop = 60,
					YCoordBottom = CurrentGridHeight
				});
			}
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
			LoadReadModel.Execute(identifier);
		}
	}
}

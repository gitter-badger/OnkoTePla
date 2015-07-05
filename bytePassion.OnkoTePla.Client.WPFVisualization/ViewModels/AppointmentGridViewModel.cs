﻿
using System;
using System.Collections.Concurrent;
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
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentGridViewModel : IAppointmentGridViewModel
	{

		// FrameworkAccess //////////////////////////////////////////////////////////////////////////////////////

		private readonly IReadModelRepository         readModelRepository;
		private readonly IConfigurationReadRepository configuration;
		private readonly ICommandBus                  commandBus;


		// GridDrawing /////////////////////////////////////////////////////////////////////////////////////

		private double currentGridWidth;
		private double currentGridHeight;

		private readonly GridLinesAndLabelPainting gridLinesAndLabelPainting;

		// AppointmentData /////////////////////////////////////////////////////////////////////////////////

		private Time timeSlotStart;
		private Time timeSlotEnd;

		private AppointmentsOfADayReadModel appointmentReadModel;
		
		private readonly ObservableCollection<ITherapyPlaceRowViewModel> therapyPlaceRows;
		private readonly IDictionary<Guid, ObservableCollection<IAppointmentViewModel>> appointmentsOnGrid;		


		// Commands ////////////////////////////////////////////////////////////////////////////////////////

		private readonly ParameterrizedCommand<AggregateIdentifier> loadReamodelCommand;
		private IAppointmentViewModel editingObject;
		private OperatingMode operatingMode;


		public AppointmentGridViewModel(IReadModelRepository readModelRepository, 
										IConfigurationReadRepository configuration,
										ICommandBus commandBus)
		{
			this.readModelRepository = readModelRepository;
			this.configuration = configuration;
			this.commandBus = commandBus;

			editingObject = null;
			operatingMode = OperatingMode.View;

			gridLinesAndLabelPainting = new GridLinesAndLabelPainting();

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
					appointmentsOnGrid[appointment.TherapyPlace.Id].Add(
						new AppointmentViewModel(commandBus, 
												 appointment, 
												 therapyPlaceRows.First(row => row.TherapyPlaceId == appointment.TherapyPlace.Id),
												 this)
					);
					break;
				}
			}
		}

		private void LoadReadModelFromId(AggregateIdentifier id)
		{
			if (appointmentReadModel != null)
			{
				appointmentReadModel.AppointmentChanged -= OnAppointmentChanged;
				appointmentReadModel.Dispose();

				therapyPlaceRows.Clear();
				appointmentsOnGrid.Clear();
			}

			appointmentReadModel = readModelRepository.GetAppointmentsOfADayReadModel(id);
			appointmentReadModel.AppointmentChanged += OnAppointmentChanged;

			var updatedId = appointmentReadModel.Identifier;

			var medicalPractice = configuration.GetMedicalPracticeByIdAndVersion(updatedId.MedicalPracticeId,
																				 updatedId.PracticeVersion);

			if (!medicalPractice.HoursOfOpening.IsOpen(updatedId.Date))
				throw new ArgumentException();

			timeSlotStart = medicalPractice.HoursOfOpening.GetOpeningTime(updatedId.Date);
			timeSlotEnd = medicalPractice.HoursOfOpening.GetClosingTime(updatedId.Date);

			gridLinesAndLabelPainting.SetNewTimeSpan(timeSlotStart, timeSlotEnd);						

			therapyPlaceRows.Clear();

			foreach (var room in medicalPractice.Rooms)  
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					var appointmentListForTherapyPlace = new ObservableCollection<IAppointmentViewModel>();
					appointmentsOnGrid.Add(therapyPlace.Id, appointmentListForTherapyPlace);
					therapyPlaceRows.Add(new TherapyPlaceRowViewModel(appointmentListForTherapyPlace, therapyPlace,room.DisplayedColor, timeSlotStart, timeSlotEnd));
				}

			foreach (var appointment in appointmentReadModel.Appointments)
				appointmentsOnGrid[appointment.TherapyPlace.Id].Add(
					new AppointmentViewModel(commandBus, 
											 appointment, 
											 therapyPlaceRows.First(row => row.TherapyPlaceId == appointment.TherapyPlace.Id),
											 this)
				);								
		}

		public ICommand LoadReadModel
		{
			get { return loadReamodelCommand; }
		}

		public ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get { return gridLinesAndLabelPainting.TimeSlotLabels;   }}
		public ObservableCollection<TimeSlotLine>              TimeSlotLines    { get { return gridLinesAndLabelPainting.TimeSlotLines;    }}
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get { return therapyPlaceRows; }}

		public double CurrentGridWidth
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridWidth, value);
				gridLinesAndLabelPainting.SetNewGridWidth(currentGridWidth);
			}
			get { return currentGridWidth; }
		}

		public double CurrentGridHeight
		{
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref currentGridHeight, value);
				gridLinesAndLabelPainting.SetNewGridHeight(currentGridHeight);
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
			LoadReadModel.Execute(identifier);
		}
	}
}

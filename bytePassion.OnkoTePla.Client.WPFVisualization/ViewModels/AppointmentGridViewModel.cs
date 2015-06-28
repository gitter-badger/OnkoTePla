
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public class AppointmentGridViewModel : IAppointmentGridViewModel, IDisposable
	{
		private double currentGridWidth;
		private double currentGridHeight;

		private readonly AppointmentsOfADayReadModel appointmentReadModel;

		private readonly ObservableCollection<TimeSlotLabel> timeSlotLabels;
		private readonly ObservableCollection<TimeSlotLine> timeSlotLines; 

		private readonly Time startTime;
		private readonly Time endTime;

		public AppointmentGridViewModel(Time startTime, Time endTime, 
										AppointmentsOfADayReadModel appointmentReadModel)
		{
			timeSlotLabels = new ObservableCollection<TimeSlotLabel>();
			timeSlotLines  = new ObservableCollection<TimeSlotLine>();

			this.startTime = startTime;
			this.endTime = endTime;
			this.appointmentReadModel = appointmentReadModel;

			appointmentReadModel.AppointmentChanged += OnAppointmentChanged;
		}

		private void OnAppointmentChanged(object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			switch (appointmentChangedEventArgs.ChangeAction)
			{
				case ChangeAction.Added:
				{
					// TODO;
					break;
				}
			}
		}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels
		{
			get { return timeSlotLabels; }
		}

		public ObservableCollection<TimeSlotLine> TimeSlotLines
		{
			get { return timeSlotLines; }
		}

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
					YCoordBottom = CurrentGridHeight - 60 - 20
				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;		

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		 
		~AppointmentGridViewModel()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
					appointmentReadModel.AppointmentChanged -= OnAppointmentChanged;
								
			}
			disposed = true;						
		}
	}
}

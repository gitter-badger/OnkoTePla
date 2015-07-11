using System;
using System.Collections.ObjectModel;
using System.Linq;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper
{
	public class AppointmentGridDisplayDataSet : IDisposable
	{

		private readonly IAppointmentGridViewModel appointmentGridViewModel;
		private readonly AppointmentsOfADayReadModel appointmentReadModel;
		private readonly ObservableCollection<IAppointmentViewModel> displayedAppointments;
		private readonly ObservableCollection<ITherapyPlaceRowViewModel> therapyPlaceRows;

		private readonly GridLinesAndLabelPainting gridLinesAndLabelPainting;

		public AppointmentGridDisplayDataSet (AppointmentsOfADayReadModel appointmentReadModel, 
			MedicalPractice medicalPractice, 
			IAppointmentGridViewModel appointmentGridViewModel)
		{
			var identifier = appointmentReadModel.Identifier;

			this.appointmentReadModel = appointmentReadModel;
			this.appointmentGridViewModel = appointmentGridViewModel;
			appointmentReadModel.AppointmentChanged += ReadModelOnAppointmentChanged;

			therapyPlaceRows = new ObservableCollection<ITherapyPlaceRowViewModel>();
			displayedAppointments = new ObservableCollection<IAppointmentViewModel>();					

			if (!medicalPractice.HoursOfOpening.IsOpen(identifier.Date))
				throw new ArgumentException();

			var timeSlotStart = medicalPractice.HoursOfOpening.GetOpeningTime(identifier.Date);
			var timeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(identifier.Date);

			gridLinesAndLabelPainting = new GridLinesAndLabelPainting();
			gridLinesAndLabelPainting.SetNewTimeSpan(timeSlotStart, timeSlotEnd);

			foreach (var room in medicalPractice.Rooms)
				foreach (var therapyPlace in room.TherapyPlaces)
				{
					therapyPlaceRows.Add(new TherapyPlaceRowViewModel(therapyPlace, room.DisplayedColor, timeSlotStart, timeSlotEnd));
				}

			foreach (var appointment in appointmentReadModel.Appointments)
				AddAppointmentToGrid(appointment);
		}


		public ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get { return gridLinesAndLabelPainting.TimeSlotLabels;   }}
		public ObservableCollection<TimeSlotLine>              TimeSlotLines    { get { return gridLinesAndLabelPainting.TimeSlotLines;    }}
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get { return therapyPlaceRows;                           }}

		public AppointmentsOfADayReadModel AppointmentReadModel
		{
			get { return appointmentReadModel; }
		}

		private void ReadModelOnAppointmentChanged (object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			switch (appointmentChangedEventArgs.ChangeAction)
			{
				case ChangeAction.Added:
				{					
					AddAppointmentToGrid(appointmentChangedEventArgs.Appointment);
					break;
				}
				case ChangeAction.Deleted:
				{
					var viewModelToRemove = displayedAppointments.FirstOrDefault(
						appointmentModel => appointmentModel.AppointmentId == appointmentChangedEventArgs.Appointment.Id
						);

					if (viewModelToRemove != null)
					{
						viewModelToRemove.Dispose();
						displayedAppointments.Remove(viewModelToRemove);
					}
					break;
				}
			}
		}

		public void SetNewGridWidth(double newGridWidth)
		{
			gridLinesAndLabelPainting.SetNewGridWidth(newGridWidth);
		}

		public void SetNewGridHeight(double newGridHeight)
		{
			gridLinesAndLabelPainting.SetNewGridHeight(newGridHeight);
		}

		private void AddAppointmentToGrid (Appointment appointment)
		{
			displayedAppointments.Add(new AppointmentViewModel(appointment, therapyPlaceRows, appointmentGridViewModel));
		}	

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		 
		~AppointmentGridDisplayDataSet()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					appointmentReadModel.AppointmentChanged -= ReadModelOnAppointmentChanged;
					appointmentReadModel.Dispose();
				}

			}
			disposed = true;
		}
	}
}
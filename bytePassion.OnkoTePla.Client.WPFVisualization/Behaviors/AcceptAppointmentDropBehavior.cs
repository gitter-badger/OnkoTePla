using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using Duration = bytePassion.Lib.TimeLib.Duration;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class AcceptAppointmentDropBehavior : Behavior<FrameworkElement>
	{
		private class TimeSlot
		{
			public TimeSlot (Time begin, Time end)
			{
				Begin = begin;
				End = end;
			}

			public Time Begin { get; }
			public Time End   { get; }
		}


		public static readonly DependencyProperty ViewModelCommunicationProperty =
			DependencyProperty.Register(nameof(ViewModelCommunication),
										typeof (IViewModelCommunication),
										typeof (AcceptAppointmentDropBehavior),
										new PropertyMetadata(default(IViewModelCommunication)));

		public static readonly DependencyProperty AppointmentsProperty = 
			DependencyProperty.Register(nameof(Appointments), 
										typeof (ObservableCollection<IAppointmentViewModel>), 
										typeof (AcceptAppointmentDropBehavior), 
										new PropertyMetadata(default(ObservableCollection<IAppointmentViewModel>)));

		public static readonly DependencyProperty DataCenterProperty = 
			DependencyProperty.Register(nameof(DataCenter), 
										typeof (IDataCenter), 
										typeof (AcceptAppointmentDropBehavior), 
										new PropertyMetadata(default(IDataCenter)));

		public static readonly DependencyProperty TherapyPlaceRowIdentifierProperty = 
			DependencyProperty.Register(nameof(TherapyPlaceRowIdentifier), 
										typeof (TherapyPlaceRowIdentifier), 
										typeof (AcceptAppointmentDropBehavior), 
										new PropertyMetadata(default(TherapyPlaceRowIdentifier)));

		public TherapyPlaceRowIdentifier TherapyPlaceRowIdentifier
		{
			get { return (TherapyPlaceRowIdentifier) GetValue(TherapyPlaceRowIdentifierProperty); }
			set { SetValue(TherapyPlaceRowIdentifierProperty, value); }
		}

		public IDataCenter DataCenter
		{
			get { return (IDataCenter) GetValue(DataCenterProperty); }
			set { SetValue(DataCenterProperty, value); }
		}

		public ObservableCollection<IAppointmentViewModel> Appointments
		{
			get { return (ObservableCollection<IAppointmentViewModel>) GetValue(AppointmentsProperty); }
			set { SetValue(AppointmentsProperty, value); }
		}

		public IViewModelCommunication ViewModelCommunication
		{
			get { return (IViewModelCommunication)GetValue(ViewModelCommunicationProperty); }
			set { SetValue(ViewModelCommunicationProperty, value); }
		}

		private AppointmentModifications appointmentModification;
		private IList<TimeSlot> slots = null;
		private Time openingTime;
		private Time closingTime;
		private double gridWidth;

		private bool dropIsPossible;
		
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.DragEnter += OnDragEnter;
			AssociatedObject.DragLeave += OnDragLeave;
			AssociatedObject.DragOver  += OnDragOver;
			AssociatedObject.Drop      += OnDrop;			
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.DragEnter -= OnDragEnter;
			AssociatedObject.DragLeave -= OnDragLeave;
			AssociatedObject.DragOver  -= OnDragOver;
			AssociatedObject.Drop      -= OnDrop;
		}

		private void OnDrop(object sender, DragEventArgs dragEventArgs)
		{		
			if (dropIsPossible)
			{
				appointmentModification.CurrentLocation = TherapyPlaceRowIdentifier;

				var timeAtDropPosition = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
				var appointmentDuration = Time.GetDurationBetween(appointmentModification.BeginTime,
																	  appointmentModification.EndTime);
				var halfappointmentDuration = appointmentDuration / 2u;

				var slotAtDropPosition = GetSlot(timeAtDropPosition);			

				if (timeAtDropPosition + halfappointmentDuration > slotAtDropPosition.End)
				{
					appointmentModification.EndTime   = slotAtDropPosition.End;
					appointmentModification.BeginTime = slotAtDropPosition.End - appointmentDuration;					
				}
				else if (timeAtDropPosition - halfappointmentDuration < slotAtDropPosition.Begin)
				{
					appointmentModification.BeginTime = slotAtDropPosition.Begin;
					appointmentModification.EndTime = slotAtDropPosition.Begin + appointmentDuration;
				}
				else
				{
					appointmentModification.BeginTime = timeAtDropPosition - halfappointmentDuration;
					appointmentModification.EndTime   = timeAtDropPosition + halfappointmentDuration;
				}								
			}

			appointmentModification.ShowDisabledOverlay = false;
		}

		private void OnDragOver(object sender, DragEventArgs dragEventArgs)
		{
			if (dragEventArgs.Data.GetDataPresent(typeof(Appointment)))
			{

				if (Appointments != null)
				{
					if (slots == null)
					{
						var currentDraggedAppointment = (Appointment)dragEventArgs.Data.GetData(typeof (Appointment));
						ComputeSlots(currentDraggedAppointment.Id);

						appointmentModification = ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
							Constants.CurrentModifiedAppointmentVariable
						).Value;
					}

					var mousePositionTime = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
					var currentPointedSlot = GetSlot(mousePositionTime);

					if (currentPointedSlot != null)
					{
						var slotLength = Time.GetDurationBetween(currentPointedSlot.Begin, currentPointedSlot.End);
						var appointmentLength = Time.GetDurationBetween(appointmentModification.BeginTime, 
																		appointmentModification.EndTime);

						if (slotLength >= appointmentLength)
						{
							// TODO DropPossible
							dropIsPossible = true;														
							return;
						}
					}

					// TODO DropImpossible
					dropIsPossible = false;									
				}
			}
		}

		private void OnDragLeave(object sender, DragEventArgs dragEventArgs)
		{
			// TODO
		}		

		private void OnDragEnter (object sender, DragEventArgs dragEventArgs)
		{
			if (dragEventArgs.Data.GetDataPresent(typeof(Appointment)))
				if (Appointments != null)
				{
					var currentDraggedAppointment = (Appointment)dragEventArgs.Data.GetData(typeof (Appointment));
					ComputeSlots(currentDraggedAppointment.Id);

					appointmentModification = ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
						Constants.CurrentModifiedAppointmentVariable
					).Value;
				}

		}

		private Time GetTimeForPosition (double xPosition)
		{			
			var durationOfWholeDay = Time.GetDurationBetween(closingTime, openingTime);			

			double relativePosition = xPosition / gridWidth;

			var resultTime = openingTime + new Duration((uint)Math.Floor((durationOfWholeDay.Seconds * relativePosition)));
			return resultTime;
		}

		private TimeSlot GetSlot(Time time)
		{
			return slots.FirstOrDefault(slot => time > slot.Begin && time < slot.End);
		}

		private void ComputeSlots(Guid currentDraggedAppointmentId)
		{
			var startOfSlots = new List<Time>();
			var endOfSlots = new List<Time>();

			var sortedAppointments = Appointments.Where(appointment => appointment.Identifier != currentDraggedAppointmentId)
												 .ToList();
			sortedAppointments.Sort((appointment, appointment1) => appointment.BeginTime.CompareTo(appointment1.BeginTime));

			gridWidth = ViewModelCommunication.GetGlobalViewModelVariable<Size>(
				Constants.AppointmentGridSizeVariable
			).Value.Width;

			var currentDate = ViewModelCommunication.GetGlobalViewModelVariable<Date>(
				Constants.AppointmentGridSelectedDateVariable
			).Value;

			var currentMedicalPracticeId = ViewModelCommunication.GetGlobalViewModelVariable<Guid>(
				Constants.AppointmentGridDisplayedPracticeVariable
			).Value;

			var medicalPractice = DataCenter.GetMedicalPracticeByDateAndId(currentDate, currentMedicalPracticeId);
			openingTime = medicalPractice.HoursOfOpening.GetOpeningTime(currentDate);
			closingTime = medicalPractice.HoursOfOpening.GetClosingTime(currentDate);

			startOfSlots.Add(openingTime);

			foreach (var appointmentViewModel in sortedAppointments)
			{
				endOfSlots.Add(appointmentViewModel.BeginTime);
				startOfSlots.Add(appointmentViewModel.EndTime);
			}

			endOfSlots.Add(closingTime);

			slots = new List<TimeSlot>();

			for (int i = 0; i < startOfSlots.Count; i++)
			{
				slots.Add(new TimeSlot(startOfSlots[i], endOfSlots[i]));
			}
		}		
	}
	
}

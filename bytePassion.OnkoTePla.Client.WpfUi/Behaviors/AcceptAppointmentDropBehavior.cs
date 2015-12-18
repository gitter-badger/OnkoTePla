using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Duration = bytePassion.Lib.TimeLib.Duration;


namespace bytePassion.OnkoTePla.Client.WpfUi.Behaviors
{
    internal class AcceptAppointmentDropBehavior : Behavior<FrameworkElement>
    {
        

        public static readonly DependencyProperty AppointmentsProperty =
            DependencyProperty.Register(nameof(Appointments),
										typeof (ObservableCollection<IAppointmentViewModel>),
										typeof (AcceptAppointmentDropBehavior));

        public static readonly DependencyProperty DataCenterProperty =
            DependencyProperty.Register(nameof(DataCenter),
										typeof (IDataCenter),
										typeof (AcceptAppointmentDropBehavior));

        public static readonly DependencyProperty TherapyPlaceRowIdentifierProperty =
            DependencyProperty.Register(nameof(TherapyPlaceRowIdentifier),
										typeof (TherapyPlaceRowIdentifier),
										typeof (AcceptAppointmentDropBehavior));

	    public static readonly DependencyProperty AppointmentModificationsProperty = 
            DependencyProperty.Register(nameof(AppointmentModifications), 
                                        typeof (AppointmentModifications), 
                                        typeof (AcceptAppointmentDropBehavior));

	    public static readonly DependencyProperty AdornerControlProperty = 
            DependencyProperty.Register(nameof(AdornerControl), 
                                        typeof (AdornerControl), 
                                        typeof (AcceptAppointmentDropBehavior));

	    public static readonly DependencyProperty GridWidthProperty = 
            DependencyProperty.Register(nameof(GridWidth), 
                                        typeof (double), 
                                        typeof (AcceptAppointmentDropBehavior));

        public static readonly DependencyProperty CurrentSelectedDateProperty = 
            DependencyProperty.Register(nameof(CurrentSelectedDate), 
                                        typeof (Date), 
                                        typeof (AcceptAppointmentDropBehavior));

        public static readonly DependencyProperty CurrentMedicalPracticeIdProperty = 
            DependencyProperty.Register(nameof(CurrentMedicalPracticeId), 
                                        typeof (Guid), 
                                        typeof (AcceptAppointmentDropBehavior));

        public Guid CurrentMedicalPracticeId
        {
            get { return (Guid) GetValue(CurrentMedicalPracticeIdProperty); }
            set { SetValue(CurrentMedicalPracticeIdProperty, value); }
        }

        public Date CurrentSelectedDate
        {
            get { return (Date) GetValue(CurrentSelectedDateProperty); }
            set { SetValue(CurrentSelectedDateProperty, value); }
        }

	    public double GridWidth
	    {
	        get { return (double) GetValue(GridWidthProperty); }
	        set { SetValue(GridWidthProperty, value); }
	    }

	    public AdornerControl AdornerControl
	    {
	        get { return (AdornerControl) GetValue(AdornerControlProperty); }
	        set { SetValue(AdornerControlProperty, value); }
	    }

	    public AppointmentModifications AppointmentModifications
	    {
	        get { return (AppointmentModifications) GetValue(AppointmentModificationsProperty); }
	        set { SetValue(AppointmentModificationsProperty, value); }
	    }  

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
        		
        private IList<TimeSlot> slots = null;
        private Time openingTime;
        private Time closingTime;        

        private bool dropIsPossible;              
        private Appointment currentDraggedAppointment;

	    protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragEnter += OnDragEnter;
            AssociatedObject.DragLeave += OnDragLeave;
            AssociatedObject.DragOver  += OnDragOver;
            AssociatedObject.Drop      += OnDrop;
        }

        protected override void OnDetaching()
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
                Time newBeginTime;
                Time newEndTime;

                CalculateCorrectAppointmentPosition(dragEventArgs, out newBeginTime, out newEndTime);

                AppointmentModifications.SetNewLocation(TherapyPlaceRowIdentifier, newBeginTime, newEndTime);
             }            			                      
        }

        private void CalculateCorrectAppointmentPosition(DragEventArgs dragEventArgs, out Time newBeginTime, out Time newEndTime)
        {
            var timeAtDropPosition = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
            var appointmentDuration = new Duration(AppointmentModifications.BeginTime,
                AppointmentModifications.EndTime);
            var halfappointmentDuration = appointmentDuration / 2u;

            var slotAtDropPosition = GetSlot(timeAtDropPosition);



            if (timeAtDropPosition + halfappointmentDuration > slotAtDropPosition.End)
            {
                newEndTime = slotAtDropPosition.End;
                newBeginTime = slotAtDropPosition.End - appointmentDuration;
            }
            else if (timeAtDropPosition - halfappointmentDuration < slotAtDropPosition.Begin)
            {
                newBeginTime = slotAtDropPosition.Begin;
                newEndTime = slotAtDropPosition.Begin + appointmentDuration;
            }
            else
            {
                newBeginTime = timeAtDropPosition - halfappointmentDuration;
                newEndTime = timeAtDropPosition + halfappointmentDuration;
            }
        }

        private void OnDragOver(object sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(typeof (Appointment)))
            {
                if (Appointments != null && AdornerControl != null)
                {
                    if (slots == null)
                    {
                        ComputeSlots(currentDraggedAppointment.Id);
                    }

                    AdornerControl.NewMousePositionForAdorner(dragEventArgs.GetPosition(AdornerControl.ReferenceElement));

                    var mousePositionTime = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
                    var currentPointedSlot = GetSlot(mousePositionTime);


                    if (currentPointedSlot != null)
                    {
                        var slotLength = new Duration(currentPointedSlot.Begin, currentPointedSlot.End);
                        var appointmentLength = new Duration(AppointmentModifications.BeginTime,
                                                             AppointmentModifications.EndTime);

                        if (slotLength >= appointmentLength)
                        {
                            AdornerControl.ShowAdornerLikeDropIsPossible();
							dropIsPossible = true;
                            return;
                        }
                    }

                    AdornerControl.ShowAdornerLikeDropIsNotPossible();
					dropIsPossible = false;
                }
            }
        }

	    private void OnDragLeave(object sender, DragEventArgs dragEventArgs)
        {
            AdornerControl.ShowAdornerLikeDropIsNotPossible();           
        }

        private void OnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(typeof (Appointment)))
            {
                if (Appointments != null)
                {
                    currentDraggedAppointment = (Appointment) dragEventArgs.Data.GetData(typeof (Appointment));
                    ComputeSlots(currentDraggedAppointment.Id);
                 }
            }
        }

        private Time GetTimeForPosition(double xPosition)
        {
            var durationOfWholeDay = new Duration(closingTime, openingTime);

            double relativePosition = xPosition / GridWidth;

            var resultTime = openingTime +
                             new Duration((uint) Math.Floor((durationOfWholeDay.Seconds*relativePosition)));
            return resultTime;
        }

        private TimeSlot GetSlot(Time time)
        {
            return slots.FirstOrDefault(slot => time > slot.Begin && time < slot.End);
        }

        private void ComputeSlots(Guid currentDraggedAppointmentId)
        {
            var startOfSlots = new List<Time>();
            var endOfSlots   = new List<Time>();

            var sortedAppointments = Appointments.Where(appointment => appointment.Identifier != currentDraggedAppointmentId)
												 .ToList();
            sortedAppointments.Sort(
                (appointment, appointment1) => appointment.BeginTime.CompareTo(appointment1.BeginTime)
			);

            var medicalPractice = DataCenter.GetMedicalPracticeByDateAndId(CurrentSelectedDate, CurrentMedicalPracticeId);
            openingTime = medicalPractice.HoursOfOpening.GetOpeningTime(CurrentSelectedDate);
            closingTime = medicalPractice.HoursOfOpening.GetClosingTime(CurrentSelectedDate);

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
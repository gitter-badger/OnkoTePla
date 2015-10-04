﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.WpfUtils.Adorner;
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

        public static readonly DependencyProperty DragAdornerTemplateProperty = DependencyProperty.Register(
            "DragAdornerTemplate", typeof (DataTemplate), typeof (MoveWholeAppointmentBehavior),
            new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DragAdornerTemplate
        {
            get { return (DataTemplate) GetValue(DragAdornerTemplateProperty); }
            set { SetValue(DragAdornerTemplateProperty, value); }
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

        public IViewModelCommunication ViewModelCommunication
        {
            get { return (IViewModelCommunication) GetValue(ViewModelCommunicationProperty); }
            set { SetValue(ViewModelCommunicationProperty, value); }
        }

        private AppointmentModifications appointmentModification;
        private IList<TimeSlot> slots = null;
        private Time openingTime;
        private Time closingTime;
        private double gridWidth;

        private bool dropIsPossible;
        private UIElementAdorner adorner;
        private AdornerLayer layer;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragEnter += OnDragEnter;
            AssociatedObject.DragLeave += OnDragLeave;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragEnter -= OnDragEnter;
            AssociatedObject.DragLeave -= OnDragLeave;
            AssociatedObject.DragOver -= OnDragOver;
            AssociatedObject.Drop -= OnDrop;
        }

        private void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            if (dropIsPossible)
            {
                var timeAtDropPosition = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
                var appointmentDuration = new Duration(appointmentModification.BeginTime,
                    appointmentModification.EndTime);
                var halfappointmentDuration = appointmentDuration/2u;

                var slotAtDropPosition = GetSlot(timeAtDropPosition);

                Time newBeginTime;
                Time newEndTime;

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

                appointmentModification.SetNewLocation(TherapyPlaceRowIdentifier, newBeginTime, newEndTime);
    
            }
            if (adorner != null)
            {
                adorner.Destroy();
                adorner.Visibility = Visibility.Collapsed;
            }
            appointmentModification.ShowDisabledOverlay = false;
        }

        private void OnDragOver(object sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(typeof (Appointment)))
            {
                if (Appointments != null)
                {
                    if (slots == null)
                    {
                        var currentDraggedAppointment = (Appointment) dragEventArgs.Data.GetData(typeof (Appointment));
                        ComputeSlots(currentDraggedAppointment.Id);

                        appointmentModification =
                            ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
                                Constants.CurrentModifiedAppointmentVariable
                                ).Value;
                    }

                    var mousePositionTime = GetTimeForPosition(dragEventArgs.GetPosition(AssociatedObject).X);
                    var currentPointedSlot = GetSlot(mousePositionTime);


                    if (currentPointedSlot != null)
                    {
                        var slotLength = new Duration(currentPointedSlot.Begin, currentPointedSlot.End);
                        var appointmentLength = new Duration(appointmentModification.BeginTime,
                            appointmentModification.EndTime);
                       

                        if (slotLength >= appointmentLength)
                        {
                            adorner.UpdatePosition(dragEventArgs.GetPosition(AssociatedObject).X, dragEventArgs.GetPosition(AssociatedObject).Y);
                            dropIsPossible = true;
                            dragEventArgs.Handled = true;
                        }
                    }

                    // TODO DropImpossible
                    dropIsPossible = false;
                    dragEventArgs.Handled = true;

                }
            }
        }

        private void OnDragLeave(object sender, DragEventArgs dragEventArgs)
        {
            
            adorner.Visibility = Visibility.Collapsed;
            dragEventArgs.Handled = true;
        }

        private void OnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(typeof (Appointment)))
            {
                if (Appointments != null)
                {
                    var currentDraggedAppointment = (Appointment) dragEventArgs.Data.GetData(typeof (Appointment));
                    ComputeSlots(currentDraggedAppointment.Id);

                    appointmentModification =
                        ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
                            Constants.CurrentModifiedAppointmentVariable
                            ).Value;


                    if (adorner == null)
                    {
                        adorner = new UIElementAdorner(AssociatedObject, DragAdornerTemplate,
                            appointmentModification.OriginalAppointment,
                            AdornerLayer.GetAdornerLayer(AssociatedObject));

                    }
                    adorner.Visibility = Visibility.Visible;
                    adorner.Width = ComputeAppointmentWidth(currentDraggedAppointment);
                }
            }
            dragEventArgs.Handled = true;
        }

        private Time GetTimeForPosition(double xPosition)
        {
            var durationOfWholeDay = new Duration(closingTime, openingTime);

            double relativePosition = xPosition/gridWidth;

            var resultTime = openingTime +
                             new Duration((uint) Math.Floor((durationOfWholeDay.Seconds*relativePosition)));
            return resultTime;
        }

        private TimeSlot GetSlot(Time time)
        {
            return slots.FirstOrDefault(slot => time > slot.Begin && time < slot.End);
        }

        private double ComputeAppointmentWidth(Appointment currentDraggedAppointment)
        {
            var lengthOfOneHour = gridWidth / (new Duration(closingTime, openingTime).Seconds / 3600.0);
            var durationOfAppointment = new Duration(currentDraggedAppointment.StartTime, currentDraggedAppointment.EndTime);

            return lengthOfOneHour * (durationOfAppointment.Seconds / 3600.0);
        }

        private void ComputeSlots(Guid currentDraggedAppointmentId)
        {
            var startOfSlots = new List<Time>();
            var endOfSlots = new List<Time>();

            var sortedAppointments =
                Appointments.Where(appointment => appointment.Identifier != currentDraggedAppointmentId)
                    .ToList();
            sortedAppointments.Sort(
                (appointment, appointment1) => appointment.BeginTime.CompareTo(appointment1.BeginTime));

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
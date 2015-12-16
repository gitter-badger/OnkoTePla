using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Domain.Events;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles
{
    public class DomainEventSerializationDouble
	{
		public enum EventType
		{
			Added,
			Replaced,
			Removed
		}

		public DomainEventSerializationDouble()
		{			 
		}

		public DomainEventSerializationDouble (DomainEvent domainEvent)
		{
			AggregateId = new AggregateIdentifierSerializationDouble(domainEvent.AggregateId);
			AggregateVersion = domainEvent.AggregateVersion;
			UserId = domainEvent.UserId;
			PatientId = domainEvent.PatientId;
			TimeStampDate = new DateSerializationDouble(domainEvent.TimeStamp.Item1);
			TimeStampTime = new TimeSerializationDouble(domainEvent.TimeStamp.Item2);
			ActionTag = domainEvent.ActionTag;
		}

		public DomainEventSerializationDouble(AppointmentAdded appointmentAddedEvent) 
			: this ((DomainEvent)appointmentAddedEvent)
		{
			DomainEventType = EventType.Added;

			Description    = appointmentAddedEvent.CreateAppointmentData.Description;
			Day            = new DateSerializationDouble(appointmentAddedEvent.CreateAppointmentData.Day);
			StartTime      = new TimeSerializationDouble(appointmentAddedEvent.CreateAppointmentData.StartTime);
			EndTime        = new TimeSerializationDouble(appointmentAddedEvent.CreateAppointmentData.EndTime);
			TherapyPlaceId = appointmentAddedEvent.CreateAppointmentData.TherapyPlaceId;
			AppointmentId  = appointmentAddedEvent.CreateAppointmentData.AppointmentId;
		}

		public DomainEventSerializationDouble(AppointmentReplaced appointmentReplacedEvent)
			: this ((DomainEvent)appointmentReplacedEvent)
		{
			DomainEventType = EventType.Replaced;

			NewDescription        = appointmentReplacedEvent.NewDescription;
			NewDate               = new DateSerializationDouble(appointmentReplacedEvent.NewDate);
			NewStartTime          = new TimeSerializationDouble(appointmentReplacedEvent.NewStartTime);
			NewEndTime            = new TimeSerializationDouble(appointmentReplacedEvent.NewEndTime);
			NewTherapyPlaceId     = appointmentReplacedEvent.NewTherapyPlaceId;
			OriginalAppointmendId = appointmentReplacedEvent.OriginalAppointmendId;
		}

		public DomainEventSerializationDouble(AppointmentDeleted appointmentDeletedEvent)
			: this ((DomainEvent)appointmentDeletedEvent)
		{
			DomainEventType = EventType.Removed;

			RemovedAppointmentId = appointmentDeletedEvent.RemovedAppointmentId;
		}

		public EventType DomainEventType { get; set; }

		public AggregateIdentifierSerializationDouble AggregateId      { get; set; }			//
		public uint                                   AggregateVersion { get; set; }			//
		public Guid                                   UserId           { get; set; }			//	common eventproperties
		public Guid                                   PatientId        { get; set; }			//
		public DateSerializationDouble                TimeStampDate    { get; set; }			//
		public TimeSerializationDouble                TimeStampTime    { get; set; }			//
		public ActionTag                              ActionTag        { get; set; }			//

		public Guid? RemovedAppointmentId { get; set; }											//  appointmentRemoved - Properties

		public string                    Description    { get; set; }							//
		public DateSerializationDouble   Day            { get; set; }							//
		public TimeSerializationDouble   StartTime      { get; set; }							//	appointmentAdded - Properties
		public TimeSerializationDouble   EndTime        { get; set; }							//
		public Guid?                     TherapyPlaceId { get; set; }							//
		public Guid?                     AppointmentId  { get; set; }							//

		public string                  NewDescription        { get; set; }						//
		public DateSerializationDouble NewDate               { get; set; }						//
		public TimeSerializationDouble NewStartTime          { get; set; }						//	appointmentReplaced - Properties
		public TimeSerializationDouble NewEndTime            { get; set; }						//
		public Guid?                   NewTherapyPlaceId     { get; set; }						//
		public Guid?                   OriginalAppointmendId { get; set; }						//

		

		public static DomainEventSerializationDouble GetDomainEventSerializationDouble(DomainEvent domainEvent)
		{
			if (domainEvent.GetType() == typeof (AppointmentAdded))
				return new DomainEventSerializationDouble((AppointmentAdded) domainEvent);
			
			if (domainEvent.GetType() == typeof (AppointmentReplaced))			
				return new DomainEventSerializationDouble((AppointmentReplaced) domainEvent);

			if (domainEvent.GetType() == typeof(AppointmentDeleted))
				return new DomainEventSerializationDouble((AppointmentDeleted) domainEvent);
			
			throw new Exception("inner error");
		}

		public DomainEvent GetDomainEvent()
		{
			switch (DomainEventType)
			{
				case EventType.Added:
				{
					return new AppointmentAdded(AggregateId.GetAggregateIdentifier(),
												AggregateVersion,
												UserId,
												new Tuple<Date, Time>(TimeStampDate.GetDate(), TimeStampTime.GetTime()),
												ActionTag,
												new CreateAppointmentData(PatientId,
																		  Description,
																		  StartTime.GetTime(),
																		  EndTime.GetTime(),
																		  Day.GetDate(),
																		  TherapyPlaceId.Value,
																		  AppointmentId.Value));
				}
				case EventType.Replaced:
				{
					return new AppointmentReplaced(AggregateId.GetAggregateIdentifier(),
												   AggregateVersion,
												   UserId,
												   PatientId,
												   new Tuple<Date, Time>(TimeStampDate.GetDate(), TimeStampTime.GetTime()),
												   ActionTag,
												   NewDescription,
												   NewDate.GetDate(),
												   NewStartTime.GetTime(),
												   NewEndTime.GetTime(),
												   NewTherapyPlaceId.Value,
												   OriginalAppointmendId.Value);
				}
				case EventType.Removed:
				{
					return new AppointmentDeleted(AggregateId.GetAggregateIdentifier(),
												  AggregateVersion,
												  UserId,
												  PatientId,
												  new Tuple<Date, Time>(TimeStampDate.GetDate(), TimeStampTime.GetTime()),
												  ActionTag,
												  RemovedAppointmentId.Value);
				}
			}
			throw new Exception("inner error");
		}
	}
}
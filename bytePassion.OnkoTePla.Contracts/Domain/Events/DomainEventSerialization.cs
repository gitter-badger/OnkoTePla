using System;
using System.Text;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Contracts.Domain.Events
{
	public static class DomainEventSerialization
	{
		private static string SerializeAppointmentAdded (AppointmentAdded addedEvent)
		{
			var sb = new StringBuilder();

			sb.Append(nameof(AppointmentAdded));                 sb.Append("|");

			sb.Append(addedEvent.AggregateId.Date);              sb.Append("|");
			sb.Append(addedEvent.AggregateId.MedicalPracticeId); sb.Append("|");
			sb.Append(addedEvent.AggregateId.PracticeVersion);   sb.Append("|");

			sb.Append(addedEvent.PatientId);                     sb.Append("|");
			sb.Append(addedEvent.Description);                   sb.Append("|");			
			sb.Append(addedEvent.StartTime);                     sb.Append("|");
			sb.Append(addedEvent.EndTime);                       sb.Append("|");
			sb.Append(addedEvent.TherapyPlaceId);                sb.Append("|");
			sb.Append(addedEvent.LabelId);						 sb.Append("|");
			sb.Append(addedEvent.AppointmentId);                 sb.Append("|");

			sb.Append(addedEvent.AggregateVersion);              sb.Append("|");
			sb.Append(addedEvent.UserId);                        sb.Append("|");
			sb.Append(addedEvent.TimeStamp.Item1);               sb.Append("|");
			sb.Append(addedEvent.TimeStamp.Item2);               sb.Append("|");
			sb.Append(addedEvent.ActionTag);                            
			
			return sb.ToString();
		}

		private static AppointmentAdded DeserializeAppointmentAdded(string s)
		{
			var eventParts = s.Split('|');

			var aggregateIdDate            = Date.Parse(eventParts[1]);
			var aggregateIdMedPracticeId   = Guid.Parse(eventParts[2]);
			var aggregateIdPracticeVersion = uint.Parse(eventParts[3]);

			var aggregateId = new AggregateIdentifier(aggregateIdDate, aggregateIdMedPracticeId, aggregateIdPracticeVersion);

			var creationDataPatientId      = Guid.Parse(eventParts[4]);
			var creationDataDescription    =            eventParts[5];			
			var creationDataStartTime      = Time.Parse(eventParts[6]);
			var creationDataEndTime        = Time.Parse(eventParts[7]);
			var creationDataTherapyPlaceId = Guid.Parse(eventParts[8]);
			var creationDataLabelId        = Guid.Parse(eventParts[9]);
			var creationDataAppointmentId  = Guid.Parse(eventParts[10]);			

			var aggregateVersion = uint.Parse(eventParts[11]);
			var userId           = Guid.Parse(eventParts[12]);
			var timeStampDate    = Date.Parse(eventParts[13]);
			var timeStampTime    = Time.Parse(eventParts[14]);

			var timeStamp = new Tuple<Date, Time>(timeStampDate, timeStampTime);

			var actionTag = (ActionTag) Enum.Parse(typeof (ActionTag), eventParts[15]);

			return new AppointmentAdded(aggregateId, aggregateVersion, userId, timeStamp, actionTag,
										creationDataPatientId,
										creationDataDescription,
										creationDataStartTime,
										creationDataEndTime,										
										creationDataTherapyPlaceId,
										creationDataLabelId,
										creationDataAppointmentId);
		}

		private static string SerializeAppointmentDeleted (AppointmentDeleted deletedEvent)
		{
			var sb = new StringBuilder();

			sb.Append(nameof(AppointmentDeleted));                 sb.Append("|");

			sb.Append(deletedEvent.AggregateId.Date);              sb.Append("|");
			sb.Append(deletedEvent.AggregateId.MedicalPracticeId); sb.Append("|");
			sb.Append(deletedEvent.AggregateId.PracticeVersion);   sb.Append("|");

			sb.Append(deletedEvent.RemovedAppointmentId);          sb.Append("|");

			sb.Append(deletedEvent.PatientId);					   sb.Append("|");
			sb.Append(deletedEvent.AggregateVersion);              sb.Append("|");
			sb.Append(deletedEvent.UserId);                        sb.Append("|");
			sb.Append(deletedEvent.TimeStamp.Item1);               sb.Append("|");
			sb.Append(deletedEvent.TimeStamp.Item2);               sb.Append("|");
			sb.Append(deletedEvent.ActionTag);                            
			

			return sb.ToString();
		}

		private static AppointmentDeleted DeserializeAppointmentDeleted(string s)
		{
			var eventParts = s.Split('|');

			var aggregateIdDate            = Date.Parse(eventParts[1]);
			var aggregateIdMedPracticeId   = Guid.Parse(eventParts[2]);
			var aggregateIdPracticeVersion = uint.Parse(eventParts[3]);

			var aggregateId = new AggregateIdentifier(aggregateIdDate, aggregateIdMedPracticeId, aggregateIdPracticeVersion);

			var removedAppointemtnId      = Guid.Parse(eventParts[4]);

			var patientId        = Guid.Parse(eventParts[5]);
			var aggregateVersion = uint.Parse(eventParts[6]);
			var userId           = Guid.Parse(eventParts[7]);
			var timeStampDate    = Date.Parse(eventParts[8]);
			var timeStampTime    = Time.Parse(eventParts[9]);

			var timeStamp = new Tuple<Date, Time>(timeStampDate, timeStampTime);

			var actionTag = (ActionTag) Enum.Parse(typeof (ActionTag), eventParts[10]);

			return new AppointmentDeleted(aggregateId, aggregateVersion, userId, patientId, 
										  timeStamp, actionTag, removedAppointemtnId);
		}

		private static string SerializeAppointmentReplaced (AppointmentReplaced replacedEvent)
		{
			var sb = new StringBuilder();

			sb.Append(nameof(AppointmentReplaced));                 sb.Append("|");

			sb.Append(replacedEvent.AggregateId.Date);              sb.Append("|");
			sb.Append(replacedEvent.AggregateId.MedicalPracticeId); sb.Append("|");
			sb.Append(replacedEvent.AggregateId.PracticeVersion);   sb.Append("|");

			sb.Append(replacedEvent.NewDescription);                sb.Append("|");
			sb.Append(replacedEvent.NewDate);                       sb.Append("|");
			sb.Append(replacedEvent.NewStartTime);                  sb.Append("|");
			sb.Append(replacedEvent.NewEndTime);                    sb.Append("|");
			sb.Append(replacedEvent.NewTherapyPlaceId);             sb.Append("|");
			sb.Append(replacedEvent.NewLabelId);					sb.Append("|");
			sb.Append(replacedEvent.OriginalAppointmendId);         sb.Append("|");

			sb.Append(replacedEvent.PatientId);                     sb.Append("|");
			sb.Append(replacedEvent.AggregateVersion);              sb.Append("|");
			sb.Append(replacedEvent.UserId);                        sb.Append("|");
			sb.Append(replacedEvent.TimeStamp.Item1);               sb.Append("|");
			sb.Append(replacedEvent.TimeStamp.Item2);               sb.Append("|");
			sb.Append(replacedEvent.ActionTag);

			return sb.ToString();
		}

		private static AppointmentReplaced DeserializeAppointmentReplaced (string s)
		{
			var eventParts = s.Split('|');

			var aggregateIdDate            = Date.Parse(eventParts[1]);
			var aggregateIdMedPracticeId   = Guid.Parse(eventParts[2]);
			var aggregateIdPracticeVersion = uint.Parse(eventParts[3]);

			var aggregateId = new AggregateIdentifier(aggregateIdDate, aggregateIdMedPracticeId, aggregateIdPracticeVersion);

			var newDescription        =            eventParts[4];
			var newDate               = Date.Parse(eventParts[5]);
			var newStartTime          = Time.Parse(eventParts[6]);
			var newEndTime            = Time.Parse(eventParts[7]);
			var newTherpyPlaceId      = Guid.Parse(eventParts[8]);
			var newLabelId            = Guid.Parse(eventParts[9]);
			var originalAppointmentId = Guid.Parse(eventParts[10]);

			var patientId        = Guid.Parse(eventParts[11]);
			var aggregateVersion = uint.Parse(eventParts[12]);
			var userId           = Guid.Parse(eventParts[13]);
			var timeStampDate    = Date.Parse(eventParts[14]);
			var timeStampTime    = Time.Parse(eventParts[15]);

			var timeStamp = new Tuple<Date, Time>(timeStampDate, timeStampTime);

			var actionTag = (ActionTag) Enum.Parse(typeof (ActionTag), eventParts[16]);

			return new AppointmentReplaced(aggregateId, aggregateVersion, userId, patientId,
										   timeStamp, actionTag, 
										   newDescription, newDate, newStartTime, newEndTime, 
										   newTherpyPlaceId, newLabelId, originalAppointmentId);
		}
		
		public static string Serialize(DomainEvent domainEvent)
		{
			if (domainEvent.GetType() == typeof(AppointmentAdded))    return SerializeAppointmentAdded   ((AppointmentAdded)    domainEvent); 
			if (domainEvent.GetType() == typeof(AppointmentDeleted))  return SerializeAppointmentDeleted ((AppointmentDeleted)  domainEvent); 
			if (domainEvent.GetType() == typeof(AppointmentReplaced)) return SerializeAppointmentReplaced((AppointmentReplaced) domainEvent);
			
			throw new ArgumentException();
		}

		public static DomainEvent Deserialize(string s)
		{
			var eventType = GetEventType(s);

			switch (eventType)
			{
				case nameof(AppointmentAdded):    return DeserializeAppointmentAdded(s);
				case nameof(AppointmentDeleted):  return DeserializeAppointmentDeleted(s);
				case nameof(AppointmentReplaced): return DeserializeAppointmentReplaced(s);

				default:
					throw new ArgumentException("this is not a serialized event");
			}
		}

		private static string GetEventType (string s)
		{
			var index = s.IndexOf("|", StringComparison.Ordinal);

			if (index == -1)
				throw new ArgumentException("inner error @ event deserialization");

			return s.Substring(0, index);
		}
	}
}

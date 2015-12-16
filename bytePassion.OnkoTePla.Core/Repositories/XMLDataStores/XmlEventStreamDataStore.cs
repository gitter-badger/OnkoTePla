using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;
using bytePassion.OnkoTePla.Client.Core.Repositories.EventStore;
using System;
using System.Collections.Generic;
using System.Xml;

namespace bytePassion.OnkoTePla.Client.Core.Repositories.XMLDataStores
{
	public class XmlEventStreamDataStore : IPersistenceService<IEnumerable<EventStream<AggregateIdentifier>>>
	{

		private static XmlWriterSettings WriterSettings
		{
			get
			{
				return new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "  ",
					NewLineChars = "\r\n",
					NewLineHandling = NewLineHandling.Replace
				};
			}
		}

		private const string XmlRoot				        = "eventStreams";
		private const string CountAttribute			        = "count";
		private const string EventStream  		            = "eventStream";
		private const string ConfigVersionAttribute         = "configVersion";
		private const string Event       			        = "event";
		private const string DateAttribute				    = "date";
		private const string MedicalPracticeAttribute       = "medicalPracticeId";
		private const string AggregateVersionAttribute      = "aggregateVersion";		
														    
		private const string AppointmentAddedEvent          = "appointmentAddedEvent";
		private const string AppointmentReplacedEvent       = "appointmentReplacedEvent";
		private const string AppointmentDeletedEvent        = "appointmentDeletedEvent";
														    
		private const string PatientIdAttribute		        = "patientId";
		private const string AppointmentIdAttribute         = "appointmentId";
		private const string DescriptionAttribute	        = "description";
		private const string StartTimeAttribute		        = "startTime";
		private const string EndTimeAttribute		        = "endTime";
		private const string TherapyPlaceIdAttribute        = "therapyPlaceId";
		private const string OriginalAppointmentIdAttribute = "originalAppointmentId";
		private const string UserIdAttribute                = "userId";
		private const string TimeStampTimeAttribute         = "timeStampTime";
		private const string TimeStampDateAttribute         = "timeStampDate";
		private const string ActionTagAttribute             = "actionTag";

		private readonly string filename;

		public XmlEventStreamDataStore(string filename)
		{
			this.filename = filename;			
		}

		public void Persist (IEnumerable<EventStream<AggregateIdentifier>> eventStreams)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();
			
				writer.WriteStartElement(XmlRoot);				

				foreach (var eventStream in eventStreams)
				{
					WriteEventStream(writer, eventStream);
				}

				writer.WriteEndElement();

			writer.WriteEndDocument();			
			writer.Close();
		}

		private static void WriteEventStream (XmlWriter writer, EventStream<AggregateIdentifier> eventStream)
		{
			writer.WriteStartElement(EventStream);
			writer.WriteAttributeString(CountAttribute, eventStream.EventCount.ToString());
			writer.WriteAttributeString(DateAttribute, eventStream.Id.Date.ToString());
			writer.WriteAttributeString(ConfigVersionAttribute, eventStream.Id.PracticeVersion.ToString());
			writer.WriteAttributeString(MedicalPracticeAttribute, eventStream.Id.MedicalPracticeId.ToString());			

			foreach (var domainEvent in eventStream.Events)
			{
				WriteEvent(writer, domainEvent);
			}
				
			writer.WriteEndElement();
		}

		private static void WriteEvent(XmlWriter writer, DomainEvent @event)
		{
			writer.WriteStartElement(Event);			
			writer.WriteAttributeString(AggregateVersionAttribute, @event.AggregateVersion.ToString());	
			writer.WriteAttributeString(UserIdAttribute,           @event.UserId.ToString());
			writer.WriteAttributeString(PatientIdAttribute,        @event.PatientId.ToString());
			writer.WriteAttributeString(TimeStampDateAttribute,    @event.TimeStamp.Item1.ToString());
			writer.WriteAttributeString(TimeStampTimeAttribute,    @event.TimeStamp.Item2.ToString());
			writer.WriteAttributeString(ActionTagAttribute,        @event.ActionTag.ToString());
			
			if (@event is AppointmentAdded)    WriteEvent(writer, (AppointmentAdded)   @event);
			if (@event is AppointmentReplaced) WriteEvent(writer, (AppointmentReplaced)@event);
			if (@event is AppointmentDeleted)  WriteEvent(writer, (AppointmentDeleted) @event);
					
			writer.WriteEndElement();
		}

		private static void WriteEvent(XmlWriter writer, AppointmentDeleted @event)
		{
			writer.WriteStartElement(AppointmentDeletedEvent);

			writer.WriteAttributeString(AppointmentIdAttribute, @event.RemovedAppointmentId.ToString());

			writer.WriteEndElement();
		}

		private static void WriteEvent(XmlWriter writer, AppointmentReplaced @event)
		{
			writer.WriteStartElement(AppointmentReplacedEvent);

			writer.WriteAttributeString(DescriptionAttribute,           @event.NewDescription);
			writer.WriteAttributeString(DateAttribute,                  @event.NewDate.ToString());
			writer.WriteAttributeString(StartTimeAttribute,             @event.NewStartTime.ToString());
			writer.WriteAttributeString(EndTimeAttribute,               @event.NewEndTime.ToString());
			writer.WriteAttributeString(TherapyPlaceIdAttribute,        @event.NewTherapyPlaceId.ToString());
			writer.WriteAttributeString(OriginalAppointmentIdAttribute, @event.OriginalAppointmendId.ToString());

			writer.WriteEndElement();
		}

		private static void WriteEvent(XmlWriter writer, AppointmentAdded @event)
		{
			writer.WriteStartElement(AppointmentAddedEvent);
			
			writer.WriteAttributeString(DescriptionAttribute,    @event.CreateAppointmentData.Description);
			writer.WriteAttributeString(TherapyPlaceIdAttribute, @event.CreateAppointmentData.TherapyPlaceId.ToString());
			writer.WriteAttributeString(StartTimeAttribute,      @event.CreateAppointmentData.StartTime.ToString());
			writer.WriteAttributeString(EndTimeAttribute,        @event.CreateAppointmentData.EndTime.ToString());
			writer.WriteAttributeString(AppointmentIdAttribute,  @event.CreateAppointmentData.AppointmentId.ToString());
			
			writer.WriteEndElement();
		}

		public IEnumerable<EventStream<AggregateIdentifier>> Load ()
		{

			IList<EventStream<AggregateIdentifier>> eventStreams = new List<EventStream<AggregateIdentifier>>();			

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				if (reader.IsEmptyElement) return eventStreams;

				while (reader.Read())
				{
					

					if (reader.NodeType != XmlNodeType.Element || reader.Name != EventStream) continue;
					if (!reader.HasAttributes) continue;

					var eventCount = 0;
					var date = Date.Dummy;
					var configVersion = 0u;
					var medicalPracticeId = new Guid();

					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{							
							case ConfigVersionAttribute:   configVersion     = UInt32.Parse(reader.Value); break;
							case CountAttribute:           eventCount        = Int32.Parse(reader.Value);  break;
							case DateAttribute:            date              = Date.Parse(reader.Value);   break;
							case MedicalPracticeAttribute: medicalPracticeId = Guid.Parse(reader.Value);   break;
						}
					}

					var id = new AggregateIdentifier(date, medicalPracticeId, configVersion);
					var events = AcceptEventStream(reader,eventCount, id);
					eventStreams.Add(new EventStream<AggregateIdentifier>(id, events)); 
				}
			}
			reader.Close();

			return eventStreams;
		}		

		private static IEnumerable<DomainEvent> AcceptEventStream(XmlReader reader, int eventStreamElementCount, AggregateIdentifier id)
		{
			IList<DomainEvent> events = new List<DomainEvent>();

			int i = 0;
			while (i < eventStreamElementCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != Event) continue;
				i++;

				var aggrevateVersion = 1u;
				var userId = new Guid();
				var patientId = new Guid();
				var timeStampDate = String.Empty;
				var timeStampTime = String.Empty;
				var actionTag = ActionTag.RegularAction;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == AggregateVersionAttribute) aggrevateVersion = UInt32.Parse(reader.Value);
						if (reader.Name == UserIdAttribute) userId = Guid.Parse(reader.Value);
						if (reader.Name == PatientIdAttribute) patientId = Guid.Parse(reader.Value);
						if (reader.Name == TimeStampDateAttribute) timeStampDate = reader.Value;
						if (reader.Name == TimeStampTimeAttribute) timeStampTime = reader.Value;
						if (reader.Name == ActionTagAttribute) actionTag = (ActionTag) Enum.Parse(typeof (ActionTag), reader.Value);
					}
				}

				DomainEvent domainEvent = null;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case AppointmentAddedEvent:
							{
								domainEvent = AcceptAppointmentAddedEvent(reader, id, aggrevateVersion, userId, patientId,
								                                          new Tuple<Date, Time>(Date.Parse(timeStampDate),
																								Time.Parse(timeStampTime)),
								                                          actionTag);
								break;
							}
							case AppointmentReplacedEvent:
							{
								domainEvent = AcceptAppointmentModifiedEvent(reader, id, aggrevateVersion, userId, patientId,
								                                             new Tuple<Date, Time>(Date.Parse(timeStampDate),
								                                                                   Time.Parse(timeStampTime)),
								                                             actionTag);
								break;
							}
							case AppointmentDeletedEvent:
							{
								domainEvent = AcceptAppointmentDeletedEvent(reader, id, aggrevateVersion, userId, patientId,
								                                            new Tuple<Date, Time>(Date.Parse(timeStampDate),
								                                                                  Time.Parse(timeStampTime)),
								                                            actionTag);
								break;
							}
						}

						events.Add(domainEvent);
						break;
					}					
				}
			}

			return events;
		}

		private static DomainEvent AcceptAppointmentDeletedEvent(XmlReader reader, AggregateIdentifier identifier, 
																 uint aggrevateVersion, Guid userId, Guid patientId, 
																 Tuple<Date, Time> timeStamp, ActionTag actionTag)
		{
			var removedAppointmentId = new Guid();

			if (reader.HasAttributes)
			{
				while (reader.MoveToNextAttribute())
				{
					if (reader.Name == AppointmentIdAttribute) removedAppointmentId = Guid.Parse(reader.Value); 					
				}
			}

			return new AppointmentDeleted(identifier, aggrevateVersion, userId, patientId, timeStamp, actionTag, removedAppointmentId);
		}

		private static AppointmentReplaced AcceptAppointmentModifiedEvent(XmlReader reader, AggregateIdentifier identifier,
																		  uint aggregateVersion, Guid userId, Guid patientId,
																		  Tuple<Date, Time> timeStamp, ActionTag actionTag)
		{
			var newDiscription = String.Empty;
			var newDate = Date.Dummy;
			var newStartTime = Time.Dummy;
			var newEndTime = Time.Dummy;
			var newTherapyPlaceId = Guid.Empty;
			var originalAppointmentId = Guid.Empty;

			if (reader.HasAttributes)
			{
				while (reader.MoveToNextAttribute())
				{
					switch (reader.Name)
					{
						case DescriptionAttribute:           newDiscription        = reader.Value;             break;
						case DateAttribute:                  newDate               = Date.Parse(reader.Value); break;
						case StartTimeAttribute:             newStartTime          = Time.Parse(reader.Value); break;
						case EndTimeAttribute:               newEndTime            = Time.Parse(reader.Value); break;
						case TherapyPlaceIdAttribute:        newTherapyPlaceId     = Guid.Parse(reader.Value); break;
						case OriginalAppointmentIdAttribute: originalAppointmentId = Guid.Parse(reader.Value); break;
					}
				}
			}

			return new AppointmentReplaced(identifier, aggregateVersion, userId, 
										   patientId, timeStamp, actionTag,
										   newDiscription, newDate,
										   newStartTime, newEndTime, 
										   newTherapyPlaceId,
										   originalAppointmentId);
		}

		private static AppointmentAdded AcceptAppointmentAddedEvent(XmlReader reader, AggregateIdentifier identifier,
																	uint aggregateVersion, Guid userId, Guid patientId,
																	Tuple<Date, Time> timeStamp, ActionTag actionTag)
		{			
			var description   = String.Empty;
			var startTime     = new Time();
			var endTime       = new Time();
			var therpyPlaceId = new Guid();
			var appointmentId = new Guid();

			if (reader.HasAttributes)
			{
				while (reader.MoveToNextAttribute())
				{
					switch (reader.Name)
					{						
						case DescriptionAttribute:    description   = reader.Value;             break;
						case TherapyPlaceIdAttribute: therpyPlaceId = Guid.Parse(reader.Value); break;
						case StartTimeAttribute:      startTime     = Time.Parse(reader.Value); break;
						case EndTimeAttribute:        endTime       = Time.Parse(reader.Value); break;
						case AppointmentIdAttribute:  appointmentId = Guid.Parse(reader.Value); break;
					}										
				}
			}

			var createAppointmentData = new CreateAppointmentData(patientId, description, startTime, endTime, identifier.Date,
																  therpyPlaceId, appointmentId);

			return new AppointmentAdded(identifier, aggregateVersion, userId, timeStamp, actionTag, createAppointmentData);
		}
	}
}

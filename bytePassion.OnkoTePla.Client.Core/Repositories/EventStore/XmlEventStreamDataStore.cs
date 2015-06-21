using System;
using System.Collections.Generic;
using System.Xml;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Domain.Events;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class XmlEventStreamDataStore : IPersistenceService<IEnumerable<EventStream>>
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

		private const string XmlRoot				      = "eventStreams";
		private const string CountAttribute			      = "count";
		private const string EventStream  		          = "eventStream";
		private const string ConfigVersionAttribute       = "configVersion";
		private const string Event       			      = "event";
		private const string AggregateIdAttribute	      = "aggregateId";
		private const string DateAttribute				  = "date";
		private const string MedicalPracticeAttribute     = "medicalPracticeId";
		private const string AggregateVersionAttribute    = "aggregateVersion";		

		private const string AppointmentAddedEvent        = "appointmentAddedEvent";
		private const string PatientIdAttribute		      = "patientId";
		private const string AppointmentIdAttribute       = "appointmentId";
		private const string DescriptionAttribute	      = "description";
		private const string StartTimeAttribute		      = "startTime";
		private const string EndTimeAttribute		      = "endTime";
		private const string TherapyPlaceIdAttribute      = "therapyPlaceId";		
		private const string UserIdAttribute              = "userId";
		private const string TimeStampTimeAttribute       = "timeStampTime";
		private const string TimeStampDateAttribute       = "timeStampDate";

		private readonly string filename;

		public XmlEventStreamDataStore(string filename)
		{
			this.filename = filename;			
		}

		public void Persist(IEnumerable<EventStream> eventStreams)
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

		private static void WriteEventStream(XmlWriter writer, EventStream eventStream)
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
			writer.WriteAttributeString(TimeStampDateAttribute,    @event.TimeStamp.Item1.ToString());
			writer.WriteAttributeString(TimeStampTimeAttribute,    @event.TimeStamp.Item2.ToString());			

			if (@event is AppointmentAdded) WriteEvent(writer, (AppointmentAdded) @event);			
					
			writer.WriteEndElement();
		}

		private static void WriteEvent(XmlWriter writer, AppointmentModified @event)
		{
			throw new NotImplementedException();
		}

		private static void WriteEvent(XmlWriter writer, AppointmentAdded @event)
		{
			writer.WriteStartElement(AppointmentAddedEvent);

			writer.WriteAttributeString(PatientIdAttribute,      @event.CreateAppointmentData.PatientId.ToString());
			writer.WriteAttributeString(DescriptionAttribute,    @event.CreateAppointmentData.Description);
			writer.WriteAttributeString(TherapyPlaceIdAttribute, @event.CreateAppointmentData.TherapyPlaceId.ToString());
			writer.WriteAttributeString(StartTimeAttribute,      @event.CreateAppointmentData.StartTime.ToString());
			writer.WriteAttributeString(EndTimeAttribute,        @event.CreateAppointmentData.EndTime.ToString());
			writer.WriteAttributeString(AppointmentIdAttribute,  @event.CreateAppointmentData.AppointmentId.ToString());
			

			writer.WriteEndElement();
		} 

		public IEnumerable<EventStream> Load ()
		{

			IList<EventStream> eventStreams = new List<EventStream>();			

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != EventStream) continue;
					if (!reader.HasAttributes) continue;

					var eventCount = 0;
					var date = Date.Dummy;
					var configVersion = 0u;
					var medicalPractiveId = new Guid();

					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{							
							case ConfigVersionAttribute:   configVersion     = UInt32.Parse(reader.Value); break;
							case CountAttribute:           eventCount        = Int32.Parse(reader.Value);  break;
							case DateAttribute:            date              = Date.Parse(reader.Value);   break;
							case MedicalPracticeAttribute: medicalPractiveId = Guid.Parse(reader.Value);   break;
						}
					}

					var id = new AggregateIdentifier(date, medicalPractiveId, configVersion);
					var events = AcceptEventStream(reader,eventCount, id);
					eventStreams.Add(new EventStream(id, events)); 
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
				var timeStampDate = String.Empty;
				var timeStampTime = String.Empty;				

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == AggregateVersionAttribute) aggrevateVersion = UInt32.Parse(reader.Value);
						if (reader.Name == UserIdAttribute) userId = Guid.Parse(reader.Value);
						if (reader.Name == TimeStampDateAttribute) timeStampDate = reader.Value;
						if (reader.Name == TimeStampTimeAttribute) timeStampTime = reader.Value;
					
					}
				}

				DomainEvent domainEvent = null;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						switch (reader.Name)
						{
							case AppointmentAddedEvent:	domainEvent = AcceptAppointmentAddedEvent(reader, id, aggrevateVersion, userId, 
																								  new Tuple<Date, Time>(Date.Parse(timeStampDate), 
																							 							Time.Parse(timeStampTime)));
														break;
						}

						events.Add(domainEvent);
						break;
					}					
				}
			}

			return events;
		}

		private static AppointmentAdded AcceptAppointmentAddedEvent(XmlReader reader, AggregateIdentifier identifier,
																	uint aggregateVersion, Guid userId,
																	Tuple<Date, Time> timeStamp)
		{
			var patientId     = new Guid();
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
						case PatientIdAttribute:      patientId     = Guid.Parse(reader.Value); break;
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

			return new AppointmentAdded(identifier, aggregateVersion, userId, timeStamp, createAppointmentData);
		}
	}
}

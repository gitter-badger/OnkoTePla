using System;
using System.Collections.Generic;
using System.Xml;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


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
		private const string EventElement			      = "event";
		private const string AggregateIdAttribute	      = "aggregateId";
		private const string DateAttribute				  = "date";
		private const string MedicalPracticeAttribute     = "medicalPracticeId";
		private const string AggregateVersionAttribute    = "aggregateVersion";
		private const string EventIdAttribute             = "eventId";

		private const string AppointmentAddedEvent        = "appointmentAddedEvent";
		private const string PatientIdAttribute		      = "patientId";
		private const string DescriptionAttribute	      = "description";
		private const string StartTimeAttribute		      = "startTime";
		private const string EndTimeAttribute		      = "endTime";
		private const string TherapyPlaceIdAttribute      = "therapyPlaceId";
		private const string RoomIdAttribute		      = "roomId";

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

		private void WriteEventStream(XmlWriter writer, EventStream eventStream)
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

		private void WriteEvent(XmlWriter writer, DomainEvent @event)
		{
			writer.WriteStartElement(EventElement);
			writer.WriteAttributeString(AggregateIdAttribute,      @event.AggregateId.ToString());
			writer.WriteAttributeString(AggregateVersionAttribute, @event.AggregateVersion.ToString());
			writer.WriteAttributeString(EventIdAttribute,          @event.EventId.ToString());

			(this as dynamic).WriteEvent(@event);
			
			writer.WriteEndElement();
		}

		protected void WriteEvent(XmlWriter writer, AppointmentAdded @event)
		{
			writer.WriteStartElement(AppointmentAddedEvent);

			writer.WriteAttributeString(PatientIdAttribute,      @event.Patient.Id.ToString());
			writer.WriteAttributeString(DescriptionAttribute,    @event.Description);
			writer.WriteAttributeString(StartTimeAttribute,      @event.StartTime.ToString());
			writer.WriteAttributeString(EndTimeAttribute,        @event.EndTime.ToString());
			writer.WriteAttributeString(TherapyPlaceIdAttribute, @event.TherapyPlace.Id.ToString());
			writer.WriteAttributeString(RoomIdAttribute,         @event.Room.Id.ToString());

			writer.WriteEndElement();
		} 

		public IEnumerable<EventStream> Load ()
		{

			IList<EventStream> eventStreams = null;			

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

					var id = new EventStreamIdentifier(date, configVersion, medicalPractiveId);
					var events = AcceptEventStream(reader,eventCount);
					eventStreams.Add(new EventStream(id, events)); 
				}
			}
			reader.Close();

			return eventStreams;
		}		

		private IEnumerable<DomainEvent> AcceptEventStream(XmlReader reader, int eventStreamElementCount)
		{
			throw new NotImplementedException();
		}	
	}
}

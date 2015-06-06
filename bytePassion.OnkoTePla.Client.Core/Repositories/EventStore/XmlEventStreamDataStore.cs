using System;
using System.Collections.Generic;
using System.Xml;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.EventStore
{
	public class XmlEventStreamDataStore : IPersistenceService<IDictionary<Guid, IReadOnlyCollection<DomainEvent>>>
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
		private const string EventStreamElement		      = "eventStream";
		private const string EventElement			      = "event";
		private const string AggregateIdAttribute	      = "aggregateId";
		private const string AggregateVersionAttribute    = "aggregateVersion";
		private const string EventIdAttribute             = "eventId";

		private const string AppointmentAddedEventElement = "appointmentAddedEvent";
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

		public void Persist(IDictionary<Guid, IReadOnlyCollection<DomainEvent>> data)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();
			
				writer.WriteStartElement(XmlRoot);
				writer.WriteAttributeString(CountAttribute, data.Count.ToString());

					foreach (var kvp in data)
					{
						WriteEventStream(writer, kvp.Key, kvp.Value);
					}

				writer.WriteEndElement();

			writer.WriteEndDocument();			
			writer.Close();
		}

		private void WriteEventStream(XmlWriter writer, 
									  Guid aggregateId, 
									  IReadOnlyCollection<DomainEvent> eventStream)
		{
			writer.WriteStartElement(EventStreamElement);
			writer.WriteAttributeString(CountAttribute, eventStream.Count.ToString());
			writer.WriteAttributeString(AggregateIdAttribute, aggregateId.ToString());

				foreach (var domainEvent in eventStream)
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
			writer.WriteStartElement(AppointmentAddedEventElement);

			writer.WriteAttributeString(PatientIdAttribute,      @event.Patient.Id.ToString());
			writer.WriteAttributeString(DescriptionAttribute,    @event.Description);
			writer.WriteAttributeString(StartTimeAttribute,      @event.StartTime.ToString());
			writer.WriteAttributeString(EndTimeAttribute,        @event.EndTime.ToString());
			writer.WriteAttributeString(TherapyPlaceIdAttribute, @event.TherapyPlace.Id.ToString());
			writer.WriteAttributeString(RoomIdAttribute,         @event.Room.Id.ToString());

			writer.WriteEndElement();
		} 

		public IDictionary<Guid, IReadOnlyCollection<DomainEvent>> Load ()
		{

			IDictionary<Guid, IReadOnlyCollection<DomainEvent>> eventStreams = null;
			int eventStreamCount = 0;

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())						
						if (reader.Name == CountAttribute)							
							eventStreamCount = Int32.Parse(reader.Value);													
				}

				eventStreams = AcceptEventStreams(reader, eventStreamCount);
			}
			reader.Close();

			return eventStreams;
		}

		private IDictionary<Guid, IReadOnlyCollection<DomainEvent>> AcceptEventStreams(XmlReader reader, int streamCount)
		{
			IDictionary<Guid, IReadOnlyCollection<DomainEvent>> eventStreams = new Dictionary<Guid, IReadOnlyCollection<DomainEvent>>();

			int i = 0;
			while (i < streamCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != EventStreamElement) continue;
				i++;

				var eventStreamElementCount = 0;
				var aggregateId = new Guid();

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == CountAttribute)							
							eventStreamElementCount = Int32.Parse(reader.Value);
							
						if (reader.Name == AggregateIdAttribute)							
							aggregateId = Guid.Parse(reader.Value);							
					}
				}

				var eventStream = AcceptEventStream(reader, eventStreamElementCount);
				eventStreams.Add(aggregateId, eventStream);
			}
			return eventStreams;
		}

		private IReadOnlyList<DomainEvent> AcceptEventStream(XmlReader reader, int eventStreamElementCount)
		{
			throw new NotImplementedException();
		}	
	}
}

using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.Base
{
	public class DomainEvent
	{
		private readonly AggregateIdentifier aggregateId;
		private readonly uint aggregateVersion;
		private readonly Guid userId;
		private readonly Guid patientId;
		private readonly Tuple<Date, Time> timeStamp;

		public DomainEvent (AggregateIdentifier aggregateId, uint aggregateVersion, 
							Guid userId, Guid patientId, Tuple<Date, Time> timeStamp)
		{
			this.aggregateId = aggregateId;
			this.aggregateVersion = aggregateVersion;
			this.userId = userId;
			this.timeStamp = timeStamp;
			this.patientId = patientId;
		}
		
		public AggregateIdentifier   AggregateId      { get { return aggregateId;      }}
		public uint                  AggregateVersion { get { return aggregateVersion; }}
		public Guid                  UserId           { get { return userId;           }}
		public Guid                  PatientId        { get { return patientId;        }}
		public Tuple<Date, Time>     TimeStamp        { get { return timeStamp;        }} 
	}
}

using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands
{
	public class AddAppointment : DomainCommand
	{
		private readonly Patient      patient;
		private readonly string       description;
		private readonly Date         day;
		private readonly Time         startTime;
		private readonly Time         endTime;
		private readonly TherapyPlace therapyPlace;
		private readonly Room		  room;

		public AddAppointment(AggregateIdentifier aggregateId, int aggregateVersion, Guid userId, 
							  Patient patient, string description, 
							  Date day, Time startTime, Time endTime, 
							  TherapyPlace therapyPlace, Room room)
			: base(aggregateId, aggregateVersion, userId)
		{
			this.patient = patient;
			this.description = description;
			this.day = day;
			this.startTime = startTime;
			this.endTime = endTime;
			this.therapyPlace = therapyPlace;
			this.room = room;
		}

		public Patient      Patient      { get { return patient;      }}
		public string       Description  { get { return description;  }}
		public Date         Day          { get { return day;          }}
		public Time         StartTime    { get { return startTime;    }}
		public Time         EndTime      { get { return endTime;      }}
		public TherapyPlace TherapyPlace { get { return therapyPlace; }}
		public Room         Room         { get { return room;         }}
	}
}

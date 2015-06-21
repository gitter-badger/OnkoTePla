using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic
{
	public struct CreateAppointmentData
	{
		private readonly Guid   patientId;
		private readonly string description;
		private readonly Time   startTime;
		private readonly Time   endTime;
		private readonly Date   day;
		private readonly Guid   therapyPlaceId;
		private readonly Guid   appointmendId;

		public CreateAppointmentData(Guid patientId, string description, 
									 Time startTime, Time endTime, Date day, 
									 Guid therapyPlaceId, Guid appointmendId)
		{
			this.patientId = patientId;
			this.description = description;
			this.startTime = startTime;
			this.endTime = endTime;
			this.day = day;
			this.therapyPlaceId = therapyPlaceId;
			this.appointmendId = appointmendId;
		}


		public Guid   PatientId      { get { return patientId;        }}
		public string Description    { get { return description;      }}
		public Date   Day            { get { return day;              }}
		public Time   StartTime      { get { return startTime;        }}
		public Time   EndTime        { get { return endTime;          }}
		public Guid   TherapyPlaceId { get { return therapyPlaceId;   }}
		public Guid   AppointmentId  { get { return appointmendId;    }}
	}
}

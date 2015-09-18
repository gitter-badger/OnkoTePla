using System;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic
{
	public struct ReplaceAppointmentData
	{
		public ReplaceAppointmentData (string newDescription, 
									   Time newStartTime, Time newEndTime, Date newDate, 
									   Guid newTherapyPlaceId, Guid originalAppointmendId)
		{
			NewDescription = newDescription;
			NewStartTime = newStartTime;
			NewEndTime = newEndTime;
			NewDate = newDate;
			NewTherapyPlaceId = newTherapyPlaceId;
			OriginalAppointmendId = originalAppointmendId;
		}

		public string NewDescription        { get; }
		public Date   NewDate               { get; }
		public Time   NewStartTime          { get; }
		public Time   NewEndTime            { get; }
		public Guid   NewTherapyPlaceId     { get; }
		public Guid   OriginalAppointmendId { get; }
	}
}

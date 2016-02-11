﻿using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetAppointmentsOfADayRequest : NetworkMessageBase
	{
		public GetAppointmentsOfADayRequest(Date day, Guid medicalPracticeId, ConnectionSessionId sessionId, 
											Guid userId, uint aggregateVersionLimit)
			: base(NetworkMessageType.GetAppointmentsOfADayRequest)
		{
			Day = day;
			SessionId = sessionId;
			UserId = userId;
			AggregateVersionLimit = aggregateVersionLimit;
			MedicalPracticeId = medicalPracticeId;
		}

		public Date                Day                   { get; }
		public Guid                MedicalPracticeId     { get; }
		public ConnectionSessionId SessionId             { get; }
		public Guid                UserId                { get; }
		public uint                AggregateVersionLimit { get; }
		 
		public override string AsString()
		{
			return $"{SessionId};{UserId};{Day};{MedicalPracticeId};{AggregateVersionLimit}";
		}

		public static GetAppointmentsOfADayRequest Parse (string s)
		{
			var parts = s.Split(';');

			var sessionId             = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId                = Guid.Parse(parts[1]);
			var day                   = Date.Parse(parts[2]);
			var medicalPracticeId     = Guid.Parse(parts[3]);
			var aggregateVersionLimit = uint.Parse(parts[4]);
			
			return new GetAppointmentsOfADayRequest(day,medicalPracticeId, sessionId, userId, aggregateVersionLimit);
		}
	}
}
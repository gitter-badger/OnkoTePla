using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetPracticeVersionInfoRequest : NetworkMessageBase
	{
		public GetPracticeVersionInfoRequest (ConnectionSessionId sessionId, Guid userId, 
										  Guid medicalPracticeId, Date day)
			: base (NetworkMessageType.GetPracticeVersionInfoRequest)
		{			
			SessionId = sessionId;
			UserId = userId;
			MedicalPracticeId = medicalPracticeId;
			Day = day;			
		}
		
		public ConnectionSessionId SessionId         { get; }
		public Guid                UserId            { get; }
		public Guid                MedicalPracticeId { get; }
		public Date                Day               { get; }	

		public override string AsString()
		{
			return $"{SessionId};{UserId};{MedicalPracticeId};{Day}";
		}

		public static GetPracticeVersionInfoRequest Parse (string s)
		{
			var parts = s.Split(';');

			var sessionId         = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId            =                         Guid.Parse(parts[1]);
			var medicalPracticeId =                         Guid.Parse(parts[2]);
			var day               =                         Date.Parse(parts[3]);
			
			return new GetPracticeVersionInfoRequest(sessionId, userId, medicalPracticeId, day);
		}
	}
}

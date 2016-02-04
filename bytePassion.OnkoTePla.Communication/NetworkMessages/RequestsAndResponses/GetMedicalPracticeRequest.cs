using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetMedicalPracticeRequest : NetworkMessageBase
	{
		public GetMedicalPracticeRequest (ConnectionSessionId sessionId, Guid userId, 
										  Guid medicalPracticeId, uint medicalPraciceVersion)
			: base (NetworkMessageType.GetPatientListRequest)
		{			
			SessionId = sessionId;
			UserId = userId;
			MedicalPracticeId = medicalPracticeId;
			MedicalPraciceVersion = medicalPraciceVersion;
		}
		
		public ConnectionSessionId SessionId             { get; }
		public Guid                UserId                { get; }
		public Guid                MedicalPracticeId     { get; }
		public uint                MedicalPraciceVersion { get; }	

		public override string AsString()
		{
			return $"{SessionId};{UserId};{MedicalPracticeId};{MedicalPraciceVersion}";
		}

		public static GetMedicalPracticeRequest Parse (string s)
		{
			var parts = s.Split(';');

			var sessionId              = new ConnectionSessionId(Guid.Parse(parts[0]));
			var userId                 =                         Guid.Parse(parts[1]);
			var medicalPracticeId      =                         Guid.Parse(parts[2]);
			var medicalPracticeVersion =                         uint.Parse(parts[3]);
			
			return new GetMedicalPracticeRequest(sessionId, userId, medicalPracticeId, medicalPracticeVersion);
		}
	}
}

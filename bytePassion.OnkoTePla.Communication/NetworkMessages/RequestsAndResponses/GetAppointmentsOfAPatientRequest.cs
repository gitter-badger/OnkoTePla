using System;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetAppointmentsOfAPatientRequest : NetworkMessageBase
	{
		public GetAppointmentsOfAPatientRequest (Guid patientId, ConnectionSessionId sessionId, Guid userId)
			: base(NetworkMessageType.GetAppointmentsOfAPatientRequest)
		{			
			SessionId = sessionId;
			UserId = userId;
			PatientId = patientId;			
		}

		public Guid                PatientId { get; }		
		public ConnectionSessionId SessionId { get; }
		public Guid                UserId    { get; }		
		 
		public override string AsString()
		{
			return $"{SessionId};{UserId};{PatientId}";
		}

		public static GetAppointmentsOfAPatientRequest Parse (string s)
		{
			var parts = s.Split(';');

			var patientId             = Guid.Parse(parts[0]);
			var sessionId             = new ConnectionSessionId(Guid.Parse(parts[1]));
			var userId                = Guid.Parse(parts[2]);		
			
			return new GetAppointmentsOfAPatientRequest(patientId, sessionId, userId);
		}
	}
}

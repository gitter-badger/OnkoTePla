using System;
using bytePassion.Lib.Types.SemanticTypes.Base;

namespace bytePassion.OnkoTePla.Contracts.Types
{
	public class ConnectionSessionId : SemanticType<Guid>
	{
		public ConnectionSessionId(Guid value) : base(value, "")
		{
		}
		
		protected override Func<SemanticType<Guid>, SemanticType<Guid>, bool> EqualsFunc => (id1, id2) => id1.Value == id2.Value;
		protected override string StringRepresentation => Value.ToString();	
	}

	// TODO: medPracticeID, UserId, TherpyPlaceId, TherapyPlaceTypeId, AppointmentID
	// TODO: base class for SemanticType<Guid>
}

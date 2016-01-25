using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetAccessablePracticesResponse : NetworkMessageBase
	{
		public GetAccessablePracticesResponse (IReadOnlyList<Guid> accessableMedicalPractices) 
			: base(NetworkMessageType.GetAccessablePracticesResponse)
		{
			AccessableMedicalPractices = accessableMedicalPractices;
		}

		private IReadOnlyList<Guid> AccessableMedicalPractices { get; } 

		public override string AsString()
		{
			var sb = new StringBuilder();

			foreach (var accessableMedicalPractice in AccessableMedicalPractices)
			{
				sb.Append(accessableMedicalPractice);
				sb.Append(";");
			}

			return sb.ToString().Substring(0, sb.Length-1);
		}
		
		public static GetAccessablePracticesResponse Parse (string s)
		{			
			var parts = s.Split(';')
						 .Select(part => part.Trim())
						 .Select(Guid.Parse)
						 .ToList();

			return new GetAccessablePracticesResponse(parts);
		}
	}
}

using System;
using bytePassion.OnkoTePla.Contracts.Domain;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores.JsonSerializationDoubles
{
	public class AggregateIdentifierSerializationDouble
	{
		public AggregateIdentifierSerializationDouble()
		{			
		}

		public AggregateIdentifierSerializationDouble (AggregateIdentifier aggregateIdentifier)
		{
			PracticeVersion = aggregateIdentifier.PracticeVersion;
			MedicalPracticeId = aggregateIdentifier.MedicalPracticeId;
			Date = new DateSerializationDouble(aggregateIdentifier.Date);
		}

		public uint                    PracticeVersion   { get; set; }
		public Guid                    MedicalPracticeId { get; set; }
		public DateSerializationDouble Date              { get; set; }

		public AggregateIdentifier GetAggregateIdentifier()
		{
			return new AggregateIdentifier(Date.GetDate(), MedicalPracticeId, PracticeVersion);
		}
	}
}
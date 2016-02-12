using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Contracts.Domain
{
	public struct AggregateIdentifier 
	{
		private readonly uint? practiceVersion;

		public AggregateIdentifier (Date date, Guid medicalPracticeId, uint? practiceVersion=null)
		{
			Date = date;
			this.practiceVersion = practiceVersion;
			MedicalPracticeId = medicalPracticeId;
		}

		public uint PracticeVersion
		{
			get
			{
				if (practiceVersion != null) 
					return practiceVersion.Value;

				throw new InvalidOperationException("Do not try to get this version value. " +
				                                    "That's impossible. Instead... " + 
				                                    "only try to realize the truth. " +
				                                    "There is no value");
			}
		}

		public Guid MedicalPracticeId { get; }
		public Date Date              { get; }

		public override bool Equals(object obj)
		{
			return this.Equals(obj,

				// note: practiceVersion is irrelevant for equality

				(identifier1, identifier2) => identifier1.Date == identifier2.Date && 				                              
				                              identifier1.MedicalPracticeId == identifier2.MedicalPracticeId);
		}

		public override int GetHashCode()
		{
			return Date.GetHashCode() ^ 				   
			       MedicalPracticeId.GetHashCode();
		}

		public override string ToString()
		{
			return $"[{Date},{practiceVersion},{MedicalPracticeId}]";
		}

		public static bool operator == (AggregateIdentifier id1, AggregateIdentifier id2) => id1.Equals(id2);
		public static bool operator != (AggregateIdentifier id1, AggregateIdentifier id2) => !(id1 == id2);
	}
}
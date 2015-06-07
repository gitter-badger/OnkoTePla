using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Domain
{
	public struct AggregateIdentifier 
	{
		private readonly Date  date;
		private readonly uint? practiceVersion;
		private readonly Guid  medicalPracticeId;

		public AggregateIdentifier (Date date, Guid medicalPracticeId, uint? practiceVersion=null)
		{
			this.date = date;
			this.practiceVersion = practiceVersion;
			this.medicalPracticeId = medicalPracticeId;
		}

		public uint? PracticeVersion   { get { return practiceVersion;   }}
		public Guid  MedicalPracticeId { get { return medicalPracticeId; }}
		public Date  Date              { get { return date;              }}

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
			       PracticeVersion.GetHashCode() ^ 
			       MedicalPracticeId.GetHashCode();
		}

		public override string ToString()
		{
			return "[" + date + ", " + practiceVersion + ", " + MedicalPracticeId + "]";
		}

		public static bool operator ==(AggregateIdentifier id1, AggregateIdentifier id2)
		{
			return id1.Equals(id2);
		}

		public static bool operator != (AggregateIdentifier id1, AggregateIdentifier id2)
		{
			return !(id1 == id2);
		}
	}
}
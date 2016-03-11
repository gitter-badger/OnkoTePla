using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Contracts.Locking
{
	public class Lock
	{
		public Lock(Guid medicalPracticeId, Date day)
		{
			MedicalPracticeId = medicalPracticeId;
			Day = day;
		}

		public Guid MedicalPracticeId { get; }
		public Date Day				  { get; }

		public override string ToString()
		{
			return $"{Day}@ {MedicalPracticeId}";
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj, (lock1, lock2) => lock1.MedicalPracticeId == lock2.MedicalPracticeId && 
													  lock1.Day == lock2.Day);
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				return (MedicalPracticeId.GetHashCode()*397) ^ Day.GetHashCode();
			}
		}
	}
}

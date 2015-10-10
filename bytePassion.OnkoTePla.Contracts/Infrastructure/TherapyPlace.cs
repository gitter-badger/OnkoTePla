using System;
using bytePassion.Lib.FrameworkExtensions;

using static bytePassion.Lib.FrameworkExtensions.EqualsExtension;

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class TherapyPlace
	{
		public TherapyPlace(Guid id, Guid typeId, string name)
		{
			Id     = id;
			TypeId = typeId;
			Name  = name;
		}

		public Guid   Id     { get; }
		public string Name   { get; }
		public Guid   TypeId { get; }


		public override string ToString ()         => Name;
		public override int    GetHashCode ()      => Id.GetHashCode();
		public override bool   Equals (object obj) => this.Equals(obj, (therapyPlace1, therapyPlace2) => therapyPlace1.Id == therapyPlace2.Id);

		public static bool operator ==(TherapyPlace t1, TherapyPlace t2) => EqualsForEqualityOperator(t1, t2);
		public static bool operator !=(TherapyPlace t1, TherapyPlace t2) => !(t1 == t2);
	}
}

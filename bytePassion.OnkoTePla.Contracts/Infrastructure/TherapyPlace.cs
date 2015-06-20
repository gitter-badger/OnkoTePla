using System;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class TherapyPlace
	{
		private readonly Guid             id; 		
		private readonly string           name;
		private readonly TherapyPlaceType type;

		public TherapyPlace(Guid id, TherapyPlaceType type, string name)
		{
			this.id   = id;
			this.type = type;
			this.name = name;
		}

		public Guid             Id   { get { return id;   }}
		public string           Name { get { return name; }}
		public TherapyPlaceType Type { get { return type; }}
		
		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (therapyPlace1, therapyPlace2) => therapyPlace1.Id == therapyPlace2.Id);
		}

		public override int GetHashCode ()
		{
			return id.GetHashCode();
		}

		#endregion
	
	}
}

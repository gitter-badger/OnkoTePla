using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class Room
	{
		private readonly Guid id;
		private readonly string name;
		private readonly IReadOnlyList<TherapyPlace> therapyPlaces;

		public Room (Guid id, string name, IReadOnlyList<TherapyPlace> therapyPlaces)
		{
			this.id = id;			
			this.name = name;
			this.therapyPlaces = therapyPlaces;
		}

		public Guid   Id   { get { return id;   }}
		public string Name { get { return name; }}

		public IReadOnlyList<TherapyPlace> TherapyPlaces
		{
			get { return therapyPlaces; }
		}

		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (room1, room2) => room1.Id == room2.Id);
		}

		public override int GetHashCode ()
		{
			return id.GetHashCode();
		}

		#endregion
	}
}

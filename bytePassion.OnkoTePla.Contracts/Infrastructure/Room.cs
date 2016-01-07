using System;
using System.Collections.Generic;
using System.Windows.Media;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class Room
	{
		public Room (Guid id, string name, IEnumerable<TherapyPlace> therapyPlaces, Color displayColor)
		{
			Id = id;			
			Name = name;
			TherapyPlaces = therapyPlaces;
			DisplayedColor = displayColor;
		}

		public Guid   Id             { get; }
		public string Name           { get; }
		public Color  DisplayedColor { get; }

		public IEnumerable<TherapyPlace> TherapyPlaces { get; }
		
		public override string ToString    ()           => Name;
		public override bool   Equals      (object obj) => this.Equals(obj, (room1, room2) => room1.Id == room2.Id);
		public override int    GetHashCode ()           => Id.GetHashCode();


		public static bool operator ==(Room r1, Room r2) => EqualsExtension.EqualsForEqualityOperator(r1, r2);
		public static bool operator !=(Room r1, Room r2) => !(r1 == r2);
	}
}

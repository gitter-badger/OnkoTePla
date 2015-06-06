using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class Room
	{
		private readonly Guid id;
		private readonly IReadOnlyList<TherapyPlace> therapyPlaces;

		public Room(Guid id, IReadOnlyList<TherapyPlace> therapyPlaces)
		{
			this.id = id;
			this.therapyPlaces = therapyPlaces;
		}

		public Guid Id
		{
			get { return id; }
		}

		public IReadOnlyList<TherapyPlace> TherapyPlaces
		{
			get { return therapyPlaces; }
		} 
	}
}

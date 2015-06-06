using System;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class MedicalPractice
	{

		private readonly IReadOnlyList<Room> rooms;
		private readonly string name;
		private readonly uint version;
		private readonly Guid id;

		public MedicalPractice(IReadOnlyList<Room> rooms, string name, uint version, Guid id)
		{
			this.rooms = rooms;
			this.name = name;
			this.version = version;
			this.id = id;
		}

		public uint                Version { get { return version; }}
		public IReadOnlyList<Room> Rooms   { get { return rooms;   }}
		public string              Name    { get { return name;    }}
		public Guid                Id      { get { return id;      }}

		public IReadOnlyList<TherapyPlace> AllTherapyPlaces
		{
			get
			{
				return Rooms.SelectMany(room => room.TherapyPlaces)
							.ToList();
			}
		}
	}
}

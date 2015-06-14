

using System;


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
	}
}

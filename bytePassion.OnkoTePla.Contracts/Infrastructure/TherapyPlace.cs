

namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class TherapyPlace
	{
		private readonly uint             id;
		private readonly TherapyPlaceType type;

		public TherapyPlace(uint id, TherapyPlaceType type)
		{
			this.id   = id;
			this.type = type;
		}

		public uint             Id   { get { return id;   }}
		public TherapyPlaceType Type { get { return type; }}		
	}
}


using bytePassion.OnkoTePla.Contracts.Enums;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public sealed class TherapyPlaceType
	{
		private readonly string typeName;
		private readonly TherapyPlaceIconType icon;

		public TherapyPlaceType(string typeName, TherapyPlaceIconType icon)
		{
			this.typeName = typeName;
			this.icon = icon;
		}

		public string TypeName
		{
			get { return typeName; }
		}

		public TherapyPlaceIconType Icon
		{
			get { return icon; }
		}
	}
}
